/*
 * Copyright (c) 2024 SaltboxGames, Jonathan Gardner
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System.Runtime.CompilerServices;
using UnityEngine;

namespace SaltboxGames.Unity.Extensions
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Returns the Squared Distance between targets;
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SqrDistance(this Vector2 a, Vector2 b)
        {
            return (a - b).sqrMagnitude;
        }
        
        /// <summary>
        /// Returns true if the squared distance between vectors is less than or equal to the threshold.
        /// Avoids the cost of a square root.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDistanceWithin(this Vector2 a, Vector2 b, float maxDistance)
        {
            return SqrDistance(a, b) <= maxDistance * maxDistance;
        }
        
        /// <summary>
        /// Returns true if the squared distance between vectors is more than to the threshold.
        /// Avoids the cost of a square root.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDistanceOutside(this Vector2 a, Vector2 b, float maxDistance)
        {
            return SqrDistance(a, b) > maxDistance * maxDistance;
        }
    }

}
