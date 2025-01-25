using UnityEditor;
using UnityEngine;

namespace Assets.RequiredField.Scripts.Editor
{
    [CustomPropertyDrawer(typeof(RequiredFieldAttribute))]
    public class RequiredFieldDrawer : PropertyDrawer
    {
        private readonly Color red = new(.8f, .2f, .2f);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!IsObject(property))
            {
                // primitive type was annotated. Display field normally
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            // save old colors
            var oldBackgroundColor = GUI.backgroundColor;
            var oldColor = GUI.color;

            if (IsInvalid(property))
            {
                GUI.backgroundColor = red;
                GUI.color = red;
            }
            EditorGUI.PropertyField(position, property, label);

            // reset colors
            GUI.backgroundColor = oldBackgroundColor;
            GUI.color = oldColor;
        }

        private static bool IsInvalid(SerializedProperty property)
        {
            // is Reference null or missing
            return property.objectReferenceInstanceIDValue == 0 || property.objectReferenceValue == null;
        }

        private static bool IsObject(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.ObjectReference;
        }
    }
}