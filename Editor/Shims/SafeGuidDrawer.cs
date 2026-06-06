// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2024-2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

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
