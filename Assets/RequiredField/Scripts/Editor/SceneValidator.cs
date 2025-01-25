using System;
using System.Collections.Generic;
using Assets.RequiredField.Scripts;
using Assets.RequiredField.Utils;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RequiredField.Scripts.Editor
{
    public static class SceneValidator
    {
        private static List<Type> classesWithRequiredAttribute = new List<Type>();

        public static void Init()
        {
            UpdateClassesWithRequired();
        }

        public static ValidationResult ValidateScene()
        {
            bool isValid = true;
            int invalidCount = 0;

            var objects = Utils.GetObjectsFromTypeList(classesWithRequiredAttribute);
            foreach (var obj in objects)
            {
                if (Utils.IsValidObject(obj))
                    continue;

                EditorGUIUtility.PingObject(obj);
                isValid = false;
                invalidCount++;
            }

            Debug.LogFormat("<b>[RequiredFields]</b> Checked {0} Components", objects.Count);

            if (!isValid)
                Debug.LogError("<b>[RequiredFields]</b> Scene has missing references!");

            return new ValidationResult()
            {
                IsValid = isValid,
                InvalidComponentCount = invalidCount,
            };
        }

        public static List<Object> GetNotValidObjects()
        {
            var result = new List<Object>();

            var objects = Utils.GetObjectsFromTypeList(classesWithRequiredAttribute);
            foreach (var obj in objects)
            {
                if (Utils.IsValidObject(obj, log: false))
                    continue;

                result.Add(obj);
            }

            return result;
        }

        private static void UpdateClassesWithRequired()
        {
            classesWithRequiredAttribute = Utils.GetClassesWithAttribute<RequiredFieldAttribute>();
        }

        public class ValidationResult
        {
            public bool IsValid { get; set; }
            public int InvalidComponentCount { get; set; }
        }
    }
}