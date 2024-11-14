using HarmonyLib;
using MGSC;
using System;

namespace QM_PatchPrototype
{
    [HarmonyPatch(typeof(MainMenuScreen), nameof(MainMenuScreen.Awake))]
    public static class MainMenuScreen_Initialize_Patch
    {
        public static void Prefix(MainMenuScreen __instance)
        {
            Console.WriteLine("main menu awake");
        }
    }
}
