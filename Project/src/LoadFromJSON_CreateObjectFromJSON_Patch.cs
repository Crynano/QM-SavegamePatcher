using HarmonyLib;
using MGSC;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QM_PatchPrototype
{
    [HarmonyPatch(
        typeof(LoadFromJSON),
        nameof(LoadFromJSON.CreateObjectFromJSON),
        new Type[] { typeof(JSONNode), typeof(Type) })]
    internal class LoadFromJSON_CreateObjectFromJSON_Patch
    {
        private static State _state => Plugin.State;
        private static Dictionary<Type, Func<Type, JSONNode, object>> _customActivators => LoadFromJSON._customActivators;
        private static Dictionary<Type, Func<Type, JSONNode, bool>> _customValidators => LoadFromJSON._customValidators;
        private static List<Type> _ignoreParseHelperTypes => LoadFromJSON._ignoreParseHelperTypes;
        private static List<System.Reflection.Assembly> assemblyList => Plugin.AssemblyList;
        private static void Prefix(JSONNode node, Type type, ref object __result)
        {
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    JSONArray asArray = node.AsArray;
                    Type type2 = type.GetGenericArguments()[0];
                    IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type2));
                    {
                        foreach (JSONNode child in asArray.Children)
                        {
                            object obj = LoadFromJSON.CreateObjectFromJSON(child, type2);
                            if (obj != null)
                            {
                                list.Add(obj);
                            }
                        }
                        __result = list;
                        return;
                    }
                }
                if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    JSONArray asArray2 = node.AsArray;
                    Type type3 = type.GetGenericArguments()[0];
                    Type type4 = type.GetGenericArguments()[1];
                    IDictionary dictionary = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(type3, type4));
                    {
                        foreach (JSONNode child2 in asArray2.Children)
                        {
                            JSONNode node2 = child2["Key"];
                            JSONNode node3 = child2["Value"];
                            dictionary.Add(LoadFromJSON.CreateObjectFromJSON(node2, type3), LoadFromJSON.CreateObjectFromJSON(node3, type4));
                        }
                        __result = dictionary;
                        return;
                    }
                }
                Debug.LogError($"Failed load data of type {type} from JSON, unknown generic type.");
                __result = null;
                return;
            }
            if (ParseHelper.HasParserForType(type) && !_ignoreParseHelperTypes.Contains(type))
            {
                if (string.IsNullOrEmpty(node.Value) && node.Count == 0)
                {
                    __result = null;
                    return;
                }
                __result = ParseHelper.ParseByType(type, node.Value);
                return;
            }
            if (type.IsClass || MGSC.SerializationHelper.IsStruct(type))
            {
                if (string.IsNullOrEmpty(node.Value) && node.Count == 0)
                {
                    __result = null;
                    return;
                }
                bool flag = typeof(IWrapTypeOnSave).IsAssignableFrom(type);
                if (flag)
                {
                    // TODO Error is HERE!
                    //type = type.Assembly.GetType(node["Type"]);
                    var findOne = assemblyList.Find(x => x.GetType(node["Type"]) != null);
                    if (findOne != null) type = findOne.GetType(node["Type"]);
                    node = node["Content"];
                }
                object obj2 = null;
                if (_customValidators.TryGetValue(type, out var value) && !value(type, node))
                {
                    __result = null;
                    return;
                }
                if (_customActivators.TryGetValue(type, out var value2))
                {
                    obj2 = value2(type, node);
                }
                else
                {
                    foreach (KeyValuePair<Type, Func<Type, JSONNode, object>> customActivator in _customActivators)
                    {
                        if (customActivator.Key.IsAssignableFrom(type))
                        {
                            obj2 = customActivator.Value(type, node);
                            break;
                        }
                    }
                    if (obj2 == null)
                    {
                        obj2 = Activator.CreateInstance(type);
                    }
                }
                if (obj2 is IManualLoad manualLoad)
                {
                    manualLoad.OnManualLoad(node);
                }
                else
                {
                    LoadFromJSON.LoadFieldsAndProperties(obj2, node, flag);
                }
                if (obj2 is IAfterLoad afterLoad)
                {
                    afterLoad.OnAfterLoad();
                }
                __result = obj2;
                return;
            }
            Debug.LogError($"Failed load data of type {type} from JSON.");
            __result = null;
            return;
        }
    }
}
