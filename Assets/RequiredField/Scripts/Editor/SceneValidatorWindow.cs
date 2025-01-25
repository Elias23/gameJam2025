using System.Collections.Generic;
using RequiredField.Scripts.Editor.Settings;
using UnityEditor;
using UnityEngine;

namespace RequiredField.Scripts.Editor
{
    public class SceneValidatorWindow : EditorWindow
    {
        private List<Object> invalid = new();
        private bool changed;

        private void OnEnable()
        {
            Undo.undoRedoPerformed += StateChanged;
            EditorApplication.hierarchyChanged += StateChanged;
            EditorApplication.projectChanged += StateChanged;
            EditorWindow.focusedWindowChanged += StateChanged;
        }

        private void OnDisable()
        {
            Undo.undoRedoPerformed -= StateChanged;
            EditorApplication.hierarchyChanged -= StateChanged;
            EditorApplication.projectChanged -= StateChanged;
            EditorWindow.focusedWindowChanged -= StateChanged;
        }

        [MenuItem("Window/Required Fields/Missing References")]
        private static void Init()
        {
            SceneValidatorWindow window = (SceneValidatorWindow)GetWindow(typeof(SceneValidatorWindow));
            window.titleContent = new GUIContent("Missing References");
            window.Show();
        }

        [MenuItem("Window/Required Fields/Open Settings")]
        private static void OpenSettings()
        {
            Selection.activeObject = RequiredFieldSettings.Instance;
        }

        private void StateChanged() => changed = true;

        private void Update()
        {
            if (!changed) return;

            changed = false;
            Repaint(); // repaint UI, to update also when not in focus
            invalid = SceneValidator.GetNotValidObjects();
        }

        private void OnGUI()
        {
            // settings
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("Settings", EditorGUIUtility.IconContent("SettingsIcon").image), GUILayout.ExpandWidth(false)))
            {
                Selection.activeObject = RequiredFieldSettings.Instance;
            }
            GUILayout.EndHorizontal();

            if (invalid.Count == 0)
            {
                // display empty
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.LabelField("No Errors");
                EditorGUI.EndDisabledGroup();
                return;
            }

            // List all components
            foreach (var obj in invalid)
            {
                if (GUILayout.Button(obj.name))
                {
                    EditorGUIUtility.PingObject(obj);
                    Selection.activeObject = obj;
                }
            }
        }
    }
}