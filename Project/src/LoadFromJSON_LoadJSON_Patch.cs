//using HarmonyLib;
//using MGSC;
//using SimpleJSON;
//using System;
//using UnityEngine;

//namespace QM_PatchPrototype
//{
//    [HarmonyPatch(
//        typeof(LoadFromJSON),
//        nameof(LoadFromJSON.LoadJSON),
//        new Type[] { typeof(LoadFromJSON), typeof(JSONNode) })]
//    internal static class LoadFromJSON_LoadJSON_Patch
//    {
//        static State _state => Plugin.State;
//        public static void Prefix(object obj, JSONNode node)
//        {
//            if (obj is IManualLoad manualLoad)
//            {
//                manualLoad.OnManualLoad(node);
//            }
//            else
//            {
//                MGSC.LoadFromJSON.LoadFieldsAndProperties(obj, node, obj is IWrapTypeOnSave);
//            }
//            if (obj is IAfterLoad afterLoad)
//            {
//                afterLoad.OnAfterLoad();
//            }
//        }
//    }
//}
