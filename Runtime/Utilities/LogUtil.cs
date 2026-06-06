// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2024-2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System.Runtime.CompilerServices;
using UnityEngine;

namespace SaltboxGames.Unity.Utilities
{
    /// <summary>
    /// Provides small helpers for formatting Unity log messages.
    /// </summary>
    public class LogUtil
    {
        /// <summary>
        /// Wraps a message in a Unity rich-text color tag in the editor, and returns the raw message in player builds.
        /// </summary>
        /// <param name="message">The message to format.</param>
        /// <param name="color">The color to apply in editor logs.</param>
        /// <returns>The formatted message in the editor, or the original message outside the editor.</returns>
        [HideInCallstack]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Color(string message, Color color)
        {
#if UNITY_EDITOR
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>";
#else
            return message;
#endif
        }
    }
}
