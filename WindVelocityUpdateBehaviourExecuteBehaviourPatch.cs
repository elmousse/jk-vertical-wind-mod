using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JumpKing.BodyCompBehaviours;
using JumpKing.Player;
using Microsoft.Xna.Framework;
using VerticalWindMod;

namespace VerticalWindMod
{
    [HarmonyPatch(typeof(WindVelocityUpdateBehaviour), nameof(WindVelocityUpdateBehaviour.ExecuteBehaviour), MethodType.Normal)]
    public class WindVelocityUpdateBehaviourExecuteBehaviourPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var codes = new List<CodeInstruction>(instructions);

            for (var i = 0; i < codes.Count - 9; i++)
            {
                if (!(codes[i].opcode == OpCodes.Ldloc_0 &&
                    codes[i + 1].opcode == OpCodes.Ldflda &&
                    codes[i + 2].opcode == OpCodes.Ldflda &&
                    codes[i + 3].opcode == OpCodes.Dup &&
                    codes[i + 4].opcode == OpCodes.Ldind_R4 &&
                    codes[i + 5].opcode == OpCodes.Ldarg_0 &&
                    codes[i + 6].opcode == OpCodes.Ldfld &&
                    codes[i + 7].opcode == OpCodes.Callvirt &&
                    codes[i + 8].opcode == OpCodes.Add &&
                    codes[i + 9].opcode == OpCodes.Stind_R4))
                {
                    continue;
                }
                
                var xWindLabel = il.DefineLabel();
                var endLabel = il.DefineLabel();
                
                var verticalWindManagerInstanceGetterInfo = typeof(VerticalWindManager)
                    .GetProperty(nameof(VerticalWindManager.Instance))?
                    .GetGetMethod();
                
                var hasWindMethodInfo = typeof(VerticalWindManager)
                    .GetMethod(nameof(VerticalWindManager.HasWind));
                
                
                var velocityFieldInfo = typeof(BodyComp)
                    .GetField(nameof(BodyComp.Velocity));
                
                var velocityYFieldInfo = typeof(Vector2)
                    .GetField(nameof(Vector2.Y));
                
                
                var windVelocityQueryFieldInfo = typeof(WindVelocityUpdateBehaviour)
                    .GetField("m_windVelocityQuery", BindingFlags.NonPublic | BindingFlags.Instance);
                
                
                var jkAssembly = Assembly.Load("JumpKing");
                
                var typeofIWindVelocityQuery = jkAssembly
                    .GetType("JumpKing.API.IWindVelocityQuery");
                
                var getCurrentVelocityMethodInfo = typeofIWindVelocityQuery
                    .GetMethod("GetCurrentVelocity");

                
                // Check if there is vertical wind
                codes.Insert(i, new CodeInstruction(OpCodes.Call, verticalWindManagerInstanceGetterInfo));
                codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldloc_2));
                codes.Insert(i + 2, new CodeInstruction(OpCodes.Callvirt, hasWindMethodInfo));
                
                // If vertical wind true
                // Add wind velocity to velocity.Y
                codes.Insert(i + 3, new CodeInstruction(OpCodes.Ldloc_0));
                codes.Insert(i + 4, new CodeInstruction(OpCodes.Ldflda, velocityFieldInfo));
                codes.Insert(i + 5, new CodeInstruction(OpCodes.Ldflda, velocityYFieldInfo));
                codes.Insert(i + 6, new CodeInstruction(OpCodes.Dup));
                codes.Insert(i + 7, new CodeInstruction(OpCodes.Ldind_R4));
                codes.Insert(i + 8, new CodeInstruction(OpCodes.Ldarg_0));
                codes.Insert(i + 9, new CodeInstruction(OpCodes.Ldfld, windVelocityQueryFieldInfo));
                codes.Insert(i + 10, new CodeInstruction(OpCodes.Callvirt, getCurrentVelocityMethodInfo));
                codes.Insert(i + 11, new CodeInstruction(OpCodes.Add));
                codes.Insert(i + 12, new CodeInstruction(OpCodes.Stind_R4));
                
                // If vertical wind false
                // Jump to add wind velocity to velocity.X
                codes.Insert(i + 3, new CodeInstruction(OpCodes.Brfalse, xWindLabel));
                
                // At end of true block, jump to end
                codes.Insert(i + 14, new CodeInstruction(OpCodes.Br, endLabel));
                
                // Reference labels
                codes[i+15].labels.Add(xWindLabel);
                codes[i+25].labels.Add(endLabel);
                
                break;
            }
            
            return codes.AsEnumerable();
        }
    }
}