using SaltboxGames.Unity.Utilities;
using UnityEditor;
using UnityEngine;

namespace SaltboxGames.Unity.Editor.Utilities
{
    [CustomPropertyDrawer(typeof(ReadOnlyFieldAttribute))]
    public sealed class ReadOnlyFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.DisabledScope(true))
                EditorGUI.PropertyField(position, property, label, includeChildren: true);
        }
    
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
    }
}
