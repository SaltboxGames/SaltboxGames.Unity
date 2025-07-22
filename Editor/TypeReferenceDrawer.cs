
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using SaltboxGames.Unity;
using ZLinq;

[CustomPropertyDrawer(typeof(TypeReference<>), useForChildren: true)]
public class TypeReferenceDrawer : PropertyDrawer
{
    private Type[] subTypes;
    private string[] typeNames;
    private GUIContent[] displayNames;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty typeNameProp = property.FindPropertyRelative("_typeName");
        
        Type fieldType = fieldInfo.FieldType;
        if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
        {
            // We're inside a List<TypeReference<T>>
            fieldType = fieldType.GetGenericArguments()[0];
            label.text = string.Empty;
        }
        
        Type genericArg = fieldType.GetGenericArguments()[0];
        if (subTypes == null)
        {
            subTypes = TypeCache.GetTypesDerivedFrom(genericArg)
                .AsValueEnumerable()
                .Where(t => t.IsClass && !t.IsAbstract)
                .OrderBy(t => t.Name)
                .ToArray();

            typeNames = subTypes.Select(t => t.AssemblyQualifiedName).ToArray();
            displayNames = subTypes.Select(t => new GUIContent(t.FullName)).ToArray();
        }

        int currentIndex = -1;
        string currentTypeName = typeNameProp.stringValue;
        if (!string.IsNullOrEmpty(currentTypeName))
        {
            currentIndex = Array.IndexOf(typeNames, currentTypeName);
        }

        EditorGUI.BeginProperty(position, label, property);

        int newIndex = EditorGUI.Popup(position, label, currentIndex, displayNames);
        if (newIndex != currentIndex)
        {
            typeNameProp.stringValue = newIndex >= 0 ? typeNames[newIndex] : "";
        }

        EditorGUI.EndProperty();
    }
}
