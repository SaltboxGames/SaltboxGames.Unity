// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2024-2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System;
using UnityEngine;


namespace SaltboxGames.Unity.Utilities
{
    /// <summary>
    /// Marks a serialized field as read-only in supported custom property drawers.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyFieldAttribute : PropertyAttribute
    {
    }
}
