// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2024-2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System.Reflection;
using SaltboxGames.Core.Shims;
using UnityEditor;

namespace SaltboxGames.Unity.Editor.Utilities
{
    /// <summary>
    /// Provides Unity editor extension methods for serialized properties.
    /// </summary>
    public static class SerializedPropertyExtensions
    {
        /// <summary>
        /// Gets the reflected field backing a serialized property.
        /// </summary>
        /// <param name="property">The serialized property to inspect.</param>
        /// <returns>The backing field info, or <see langword="null"/> when it cannot be resolved.</returns>
        public static FieldInfo GetFieldInfo(this SerializedProperty property)
        {
            return EditorReflection.GetFieldInfoFromProperty(property);
        }

        /// <summary>
        /// Reads a serialized <see cref="SafeGuid"/> value from its integer segment fields.
        /// </summary>
        /// <param name="p">The serialized property representing a <see cref="SafeGuid"/>.</param>
        /// <returns>The reconstructed GUID value.</returns>
        public static SafeGuid GetGuidValue(this SerializedProperty p)
        {
            int s1 = p.FindPropertyRelative("segment1").intValue;
            int s2 = p.FindPropertyRelative("segment2").intValue;
            int s3 = p.FindPropertyRelative("segment3").intValue;
            int s4 = p.FindPropertyRelative("segment4").intValue;
            return SafeGuid.FromSegments(s1, s2, s3, s4);
        }

        /// <summary>
        /// Writes a <see cref="SafeGuid"/> value into its serialized integer segment fields.
        /// </summary>
        /// <param name="p">The serialized property representing a <see cref="SafeGuid"/>.</param>
        /// <param name="guid">The GUID value to write.</param>
        public static void SetGuidValue(this SerializedProperty p, SafeGuid guid)
        {
            guid.Decompose(out int s1, out int s2, out int s3, out int s4);
            p.FindPropertyRelative("segment1").intValue = s1;
            p.FindPropertyRelative("segment2").intValue = s2;
            p.FindPropertyRelative("segment3").intValue = s3;
            p.FindPropertyRelative("segment4").intValue = s4;
        }
    }
}
