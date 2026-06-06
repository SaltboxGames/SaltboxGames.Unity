// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2024-2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using SaltboxGames.Unity;

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
