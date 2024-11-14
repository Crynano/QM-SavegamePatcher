//using HarmonyLib;
//using MGSC;
//using SimpleJSON;
//using System;
//using System.Reflection;
//using UnityEngine;

//namespace QM_PatchPrototype
//{
//    [HarmonyPatch(
//        typeof(LoadFromJSON),
//        nameof(LoadFromJSON.LoadFieldsAndProperties),
//        new Type[] { typeof(object), typeof(JSONNode), typeof(bool) })]
//    internal static class LoadFromJSON_LoadFieldsAndProperties_Patch
//    {
//        static State _state => Plugin.State;

//        public static void Prefix(object obj, JSONNode node, bool deepHierarchy)
//        {
//            Type type = obj.GetType();
//            Type type2 = null;
//            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
//            do
//            {
//                PropertyInfo[] properties = type.GetProperties(bindingAttr);
//                foreach (PropertyInfo propertyInfo in properties)
//                {
//                    if (propertyInfo.GetCustomAttribute<Save>() != null && (!(type2 != null) || !(type2.GetProperty(propertyInfo.Name, bindingAttr) != null)))
//                    {
//                        JSONNode jSONNode = node[propertyInfo.Name];
//                        if (jSONNode != null)
//                        {
//                            object value = LoadFromJSON.CreateObjectFromJSON(jSONNode, propertyInfo.PropertyType);
//                            propertyInfo.SetValue(obj, value);
//                        }
//                    }
//                }
//                FieldInfo[] fields = type.GetFields(bindingAttr);
//                for (int i = 0; i < fields.Length; i++)
//                {
//                    Load(fields[i]);
//                }
//                type2 = type;
//                type = type.BaseType;
//            }
//            while (deepHierarchy && type != null && type != typeof(object));
//            void Load(FieldInfo field)
//            {
//                if (field.GetCustomAttribute<Save>() != null)
//                {
//                    JSONNode jSONNode2 = node[field.Name];
//                    if (jSONNode2 != null)
//                    {
//                        object value2 = LoadFromJSON.CreateObjectFromJSON(jSONNode2, field.FieldType);
//                        field.SetValue(obj, value2);
//                    }
//                }
//            }
//        }
//    }
//}