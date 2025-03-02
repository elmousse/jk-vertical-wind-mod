using System.Diagnostics;
using HarmonyLib;
using JumpKing;
using JumpKing.Level;
using JumpKing.Mods;

namespace VerticalWindMod
{
    [JumpKingMod("McOuille.VerticalWindMod")]
    public static class ModEntry
    {
        [BeforeLevelLoad]
        public static void BeforeLevelLoad()
        {
            var harmony = new Harmony("McOuille.VerticalWindMod");
            harmony.PatchAll();
            
            VerticalWindManager.Instance.Clear();
            LevelManager.RegisterBlockFactory(new VerticalWindFactory());
        }
        
        [OnLevelUnload]
        public static void OnLevelUnload()
        {
            VerticalWindManager.Instance.Clear();
        }
    }
}
