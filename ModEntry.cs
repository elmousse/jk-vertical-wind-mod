using System.Diagnostics;
using HarmonyLib;
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
            Debugger.Launch();
            
            Harmony.DEBUG = true;
            var harmony = new Harmony("McOuille.VerticalWindMod");
            harmony.PatchAll();
            
            VerticalWindManager.Instance.Clear();
            LevelManager.RegisterBlockFactory(new VerticalWindFactory());
        }
        
        [OnLevelStart]
        public static void OnLevelStart()
        {
            
        }
        
        [OnLevelUnload]
        public static void OnLevelUnload()
        {
            VerticalWindManager.Instance.Clear();
        }
    }
}
