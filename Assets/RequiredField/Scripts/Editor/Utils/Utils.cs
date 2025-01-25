using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.RequiredField.Scripts;
using RequiredField.Scripts.Editor.Settings;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.RequiredField.Utils
{
    public static class Utils
    {
        private static readonly BindingFlags fieldFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public static bool IsValidObject(Object obj, bool log = true)
        {
            bool isValid = true;
            foreach (var field in obj.GetType().GetFields(fieldFlags))
            {
                var attributes = field.GetCustomAttributes(typeof(RequiredFieldAttribute));
                if (!attributes.Any())
                    continue;

                var value = field.GetValue(obj);

                bool fieldInvalid = false;
                switch (value)
                {
                    // handle list & array
                    case IList list when list.Count == 0:
                    case Array array when array.Length == 0:
                        fieldInvalid = true;
                        break;

                    default:
                        {
                            // handle object reference
                            if (ReferenceEquals(value, null) || "null".Equals(value.ToString()))
                                fieldInvalid = true;

                            break;
                        }
                }

                if (!fieldInvalid)
                    continue;

                isValid = false;
                if (log) Debug.LogErrorFormat(obj, "<b>[RequiredFields]</b> Missing reference in: {0}:   " +
                                                   "<b><color=#FF6E40>{1}</b></color>", obj, field.Name);
            }

            return isValid;
        }

        public static List<Object> GetObjectsFromTypeList(IEnumerable<Type> types)
        {
            var result = new List<Object>();
            var includeInactive = RequiredFieldSettings.Instance.IncludeInactive ?
                FindObjectsInactive.Include : FindObjectsInactive.Exclude;

            foreach (var type in types)
            {
                result.AddRange(Object.FindObjectsByType(type, includeInactive, FindObjectsSortMode.None));
            }

            return result;
        }

        public static List<Type> GetClassesWithAttribute<T>() where T : Attribute
        {
            var result = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    var fields = GetFieldsWithAttribute(type, typeof(T));
                    if (fields != null && fields.Any() && type.IsSubclassOf(typeof(Component)))
                    {
                        result.Add(type);
                    }
                }
            }

            return result;
        }

        private static IEnumerable<FieldInfo> GetFieldsWithAttribute(Type classType, Type attributeType)
        {
            return classType
                .GetFields(fieldFlags)
                .Where(fieldInfo => fieldInfo.GetCustomAttributes(attributeType, false).Any());
        }
    }
}