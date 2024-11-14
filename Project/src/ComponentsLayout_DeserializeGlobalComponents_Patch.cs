//using HarmonyLib;
//using MGSC;
//using SimpleJSON;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace QM_PatchPrototype
//{
//    [HarmonyPatch(
//        typeof(ComponentsLayout),
//        nameof(ComponentsLayout.DeserializeGlobalComponents),
//        new Type[] { typeof(JSONNode) })]
//    internal static class ComponentsLayout_DeserializeGlobalComponents_Patch
//    {
//        static State _state => Plugin.State;
//        public static void Prefix(JSONNode jsonNode)
//        {
//            JSONArray asArray = jsonNode["Components"].AsArray;
//            Dictionary<Type, JSONNode> typesToNodes = new Dictionary<Type, JSONNode>();
//            foreach (JSONNode child in asArray.Children)
//            {
//                // DOUBLE?
//                //Type type = typeof(DungeonBuilder).Assembly.GetType(child["Type"]);
//                var findOne = Plugin.AssemblyList.Find(x => x.GetType(child["Type"]) != null);
//                if (findOne != null)
//                {
//                    Type type = findOne.GetType(child["Type"]);
//                    typesToNodes.Add(type, child["Content"]);
//                }
//            }
//            LoadComponent<Difficulty>();
//            LoadComponent<SpaceTime>();
//            LoadComponent<MagnumProjects>();
//            LoadComponent<ItemsPrices>();
//            LoadComponent<Mercenaries>();
//            LoadComponent<Factions>();
//            LoadComponent<Stations>();
//            LoadComponent<Missions>();
//            LoadComponent<Shippings>();
//            LoadComponent<TravelMetadata>();
//            LoadComponent<MagnumSpaceship>();
//            LoadComponent<MagnumCargo>();
//            LoadComponent<News>();
//            LoadComponent<RaidMetadata>();
//            LoadComponent<Statistics>();
//            LoadComponent<StoryTriggers>();
//            LoadComponent<SpaceTickTimers>();
//            LoadComponent<MonsterTransfer>();
//            FactionSystem.PopulateWithNewFactions(_state.Get<Factions>(), _state.Get<SpaceTime>());
//            StationSystem.PopulateWithNewStations(_state.Get<StationFactory>(), _state.Get<Stations>(), _state.Get<Factions>(), _state.Get<ItemsPrices>(), _state.Get<SpaceTime>());
//            MissionSystem.PopulateWithReverseMissions(_state.Get<MissionFactory>(), _state.Get<Missions>(), _state.Get<SpaceTime>());
//            void LoadComponent<T>() where T : class
//            {
//                T val = _state.Get<T>();
//                if (val != null)
//                {
//                    if (!typesToNodes.TryGetValue(typeof(T), out var value))
//                    {
//                        Debug.LogError($"Failed init {typeof(T)}, no node in json.");
//                    }
//                    else
//                    {
//                        val.LoadJSON(value);
//                    }
//                }
//                else
//                {
//                    Debug.LogError($"Failed load {typeof(T)} from json, no in state.");
//                }
//            }
//        }
//    }
//}
