/*
 * Copyright (c) 2024 SaltboxGames, Jonathan Gardner
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System.Runtime.CompilerServices;
using UnityEngine;

namespace SaltboxGames.Unity.Utilities
{
    public class LogUtils
    {
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
