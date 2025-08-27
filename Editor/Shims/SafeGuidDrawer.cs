using SaltboxGames.Core.Shims;
using SaltboxGames.Unity.Editor.Utilities;
using UnityEditor;
using UnityEngine;

namespace SaltboxGames.Unity.Editor.Shims
{
    [CustomPropertyDrawer(typeof(SafeGuid))]
    public class SafeGuidDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            SafeGuid current = prop.GetGuidValue();

            EditorGUI.BeginChangeCheck();
            string next = EditorGUI.TextField(pos, label, current.ToString());
            
            if (EditorGUI.EndChangeCheck())
            {
                prop.SetGuidValue(SafeGuid.Parse(next));
                prop.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
