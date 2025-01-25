using UnityEditor;
using UnityEngine.UIElements;

namespace RequiredField.Scripts.Editor.Settings
{
    public class RequiredFieldSettingsProvider : SettingsProvider
    {
        private SerializedObject m_SerializedObject;
        private SerializedProperty m_allowStartWithErrorsProperty;
        private SerializedProperty m_IncludeInactiveProperty;

        public RequiredFieldSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope) { }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            var settings = RequiredFieldSettings.GetOrCreateSettings();
            m_SerializedObject = new SerializedObject(settings);

            m_allowStartWithErrorsProperty = m_SerializedObject.FindProperty(nameof(RequiredFieldSettings.AllowStartWithErrors));
            m_IncludeInactiveProperty = m_SerializedObject.FindProperty(nameof(RequiredFieldSettings.IncludeInactive));
        }

        public override void OnGUI(string searchContext)
        {
            m_SerializedObject.Update();
            EditorGUILayout.PropertyField(m_allowStartWithErrorsProperty);
            EditorGUILayout.PropertyField(m_IncludeInactiveProperty);
            m_SerializedObject.ApplyModifiedProperties();
        }

        [SettingsProvider]
        public static SettingsProvider CreateMySettingsProvider()
        {
            var provider = new RequiredFieldSettingsProvider("Project/Required Fields", SettingsScope.Project)
            {
                keywords = GetSearchKeywordsFromGUIContentProperties<RequiredFieldSettings>(),
            };
            return provider;
        }
    }
}