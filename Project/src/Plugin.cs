using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QM_PatchPrototype
{
    public static class Plugin
    {
        public static string ModAssemblyName => Assembly.GetExecutingAssembly().GetName().Name;

        public static string ConfigPath => Path.Combine(Application.persistentDataPath, ModAssemblyName, "config.json");
        public static string ModPersistenceFolder => Path.Combine(Application.persistentDataPath, ModAssemblyName);
        public static ModConfig Config { get; private set; }

        public static State State;

        public static List<Assembly> AssemblyList;

        [Hook(ModHookType.AfterConfigsLoaded)]
        public static void AfterConfig(IModContext context)
        {
            Directory.CreateDirectory(ModPersistenceFolder);

            Config = ModConfig.LoadConfig(ConfigPath);

            State = context.State;

            AssemblyList = new List<Assembly>();
            AssemblyList = AppDomain.CurrentDomain.GetAssemblies().ToList();

            // Let's try to manually load mod assemblies?
            foreach (var userMod in State.Get<UserMods>().Values)
            {
                AssemblyList.AddRange(userMod.LoadedAssemblies);
            }

            new Harmony("Crynano_" + ModAssemblyName).PatchAll();
        }
    }
}
