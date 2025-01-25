using RequiredField.Scripts.Editor.Settings;
using UnityEditor;

namespace RequiredField.Scripts.Editor
{
    [InitializeOnLoad]
    public static class RequiredFieldEditor
    {
        static RequiredFieldEditor()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            SceneValidator.Init();
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
                Validate();
        }

        private static void Validate()
        {
            var result = SceneValidator.ValidateScene();
            if (result.IsValid) return;

            bool cancelPay = true;
            if (RequiredFieldSettings.Instance.AllowStartWithErrors)
            {
                string message = result.InvalidComponentCount == 1
                    ? $"There is 1 Component with fields missing. Continue playing anyway?"
                    : $"There are {result.InvalidComponentCount} Components with fields missing. Continue playing anyway?";
                cancelPay = !EditorUtility.DisplayDialog("Required Fields",
                        message,
                        "Continue",
                        "Cancel");
            }

            if (cancelPay)
            {
                EditorApplication.isPlaying = false;
            }
        }
    }
}