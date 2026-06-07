// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2024-2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using SaltboxGames.Unity.Utilities;
using UnityEditor;
using UnityEngine;

namespace SaltboxGames.Unity.Editor.Utilities
{
    /// <summary>
    /// Property drawer that displays fields marked with <see cref="ReadOnlyFieldAttribute"/> in a disabled state.
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadOnlyFieldAttribute))]
    public sealed class ReadOnlyFieldDrawer : PropertyDrawer
    {
        /// <inheritdoc />
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.DisabledScope(true))
                EditorGUI.PropertyField(position, property, label, includeChildren: true);
        }

        /// <inheritdoc />
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
    }
}
