/*
 * Copyright (c) 2024 SaltboxGames, Jonathan Gardner
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */


using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaltboxGames.Unity.FX
{
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
