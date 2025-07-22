#if ADDRESSABLES_2

using System;
using UnityEditor;
using UnityEngine;

namespace SaltboxGames.Unity.Editor
{
    [CustomPropertyDrawer(typeof(AssetReferenceScene))]
    public class AssetReferenceSceneDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty sceneGUID = property.FindPropertyRelative("_sceneGuid");
            if (sceneGUID == null)
            {
                EditorGUI.LabelField(position, label.text, "Invalid AssetReference");
                return;
            }
            
            Type fieldType = fieldInfo.FieldType;
            if (fieldType.IsGenericType)
            {
                // Lists don't need the name;
                label.text = String.Empty;
            }

            string guid = sceneGUID.stringValue;
            string path = AssetDatabase.GUIDToAssetPath(guid);

            SceneAsset current = null;
            if (!string.IsNullOrEmpty(path))
            {
                current = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
            }

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();
            SceneAsset selected = (SceneAsset)EditorGUI.ObjectField(position, label, current, typeof(SceneAsset), false);
            if (EditorGUI.EndChangeCheck())
            {
                if (selected != null)
                {
                    string selectedPath = AssetDatabase.GetAssetPath(selected);
                    string newGuid = AssetDatabase.AssetPathToGUID(selectedPath);
                    sceneGUID.stringValue = newGuid;
                }
                else
                {
                    sceneGUID.stringValue = string.Empty;
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
#endif