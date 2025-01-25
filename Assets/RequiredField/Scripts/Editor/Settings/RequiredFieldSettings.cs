using UnityEditor;
using UnityEngine;

namespace RequiredField.Scripts.Editor.Settings
{
    public class RequiredFieldSettings : ScriptableObject
    {
        [Tooltip("Allow starting the application with missing references.")]
        public bool AllowStartWithErrors = true;

        [Tooltip("Also validate inactive objects and components")]
        public bool IncludeInactive = true;

        public const string SettingsPath = "Assets/RequiredField/RequiredFieldSettings.asset";

        private static RequiredFieldSettings _instance;

        public static RequiredFieldSettings Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = GetOrCreateSettings();
                return _instance;
            }
        }

        public static RequiredFieldSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<RequiredFieldSettings>(SettingsPath);

            if (settings != null) return settings;

            settings = CreateInstance<RequiredFieldSettings>();
            AssetDatabase.CreateAsset(settings, SettingsPath);
            AssetDatabase.SaveAssets();
            return settings;
        }
    }
}