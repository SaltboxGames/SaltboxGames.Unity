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

namespace SaltboxGames.Unity.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Vector2"/>.
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// Returns the squared distance between two vectors.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <returns>The squared distance between <paramref name="a"/> and <paramref name="b"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SqrDistance(this Vector2 a, Vector2 b)
        {
            return (a - b).sqrMagnitude;
        }
        
        /// <summary>
        /// Returns true if the squared distance between vectors is less than or equal to the threshold.
        /// Avoids the cost of a square root.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <param name="maxDistance">The maximum allowed distance.</param>
        /// <returns><see langword="true"/> when the vectors are within <paramref name="maxDistance"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDistanceWithin(this Vector2 a, Vector2 b, float maxDistance)
        {
            return SqrDistance(a, b) <= maxDistance * maxDistance;
        }
        
        /// <summary>
        /// Returns true if the squared distance between vectors is greater than the threshold.
        /// Avoids the cost of a square root.
        /// </summary>
        /// <param name="a">The first vector.</param>
        /// <param name="b">The second vector.</param>
        /// <param name="maxDistance">The maximum allowed distance.</param>
        /// <returns><see langword="true"/> when the vectors are farther apart than <paramref name="maxDistance"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDistanceOutside(this Vector2 a, Vector2 b, float maxDistance)
        {
            return SqrDistance(a, b) > maxDistance * maxDistance;
        }
    }
}
