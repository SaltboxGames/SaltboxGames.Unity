// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2024-2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

#if CINEMACHINE

using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaltboxGames.Unity.FX
{
    /// <summary>
    /// Provides a time-scale based freeze-frame effect.
    /// </summary>
    public class FreezeEffect
    {
        private static bool _isFreezing;

        /// <summary>
        /// Freezes the game using Time.timeScale for a short real-time duration.
        /// </summary>
        /// <param name="duration">Duration of the freeze in seconds (real-time).</param>
        public static async UniTaskVoid Freeze(float duration)
        {
            if (_isFreezing || duration <= 0f)
                return;

            _isFreezing = true;

            float originalTimeScale = Time.timeScale;
            Time.timeScale = 0f;

            await UniTask.Delay((int)(duration * 1000), DelayType.Realtime);

            Time.timeScale = originalTimeScale;
            _isFreezing = false;
        }
    }
}

#endif
