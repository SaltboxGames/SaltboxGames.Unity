// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2024-2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

#if CINEMACHINE

using Unity.Cinemachine;
using UnityEngine;

namespace SaltboxGames.Unity.FX
{
    /// <summary>
    /// Cinemachine extension that applies random positional shake while active.
    /// </summary>
    [RequireComponent(typeof(CinemachineCamera))]
    public class CameraShakeEffect : CinemachineExtension
    {
        private float magnitude;
        private float duration;
        private float remaining;
        private bool useUnscaledTime;
        private System.Random rng = new System.Random();

        /// <inheritdoc />
        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage,
            ref CameraState state,
            float deltaTime)
        {
            if (stage != CinemachineCore.Stage.Noise || remaining <= 0f)
            {
                return;
            }

            Vector2 shakeOffset = Random.insideUnitCircle * magnitude;
            state.PositionCorrection += (Vector3)shakeOffset;

            remaining -= useUnscaledTime ? Time.unscaledDeltaTime : deltaTime;
        }

        /// <summary>
        /// Starts or refreshes a scaled-time shake using the greater current and provided magnitude and duration.
        /// </summary>
        /// <param name="magnitude">The shake magnitude.</param>
        /// <param name="duration">The shake duration in seconds.</param>
        public void Shake(float magnitude, float duration)
        {
            this.magnitude = Mathf.Max(this.magnitude, magnitude);
            this.duration = Mathf.Max(this.duration, duration);
            remaining = duration;
            useUnscaledTime = false;
        }

        /// <summary>
        /// Starts or refreshes an unscaled-time shake using the greater current and provided magnitude and duration.
        /// </summary>
        /// <param name="magnitude">The shake magnitude.</param>
        /// <param name="duration">The shake duration in seconds.</param>
        public void ShakeUnscaled(float magnitude, float duration)
        {
            this.magnitude = Mathf.Max(this.magnitude, magnitude);
            this.duration = Mathf.Max(this.duration, duration);
            remaining = duration;
            useUnscaledTime = true;
        }

        /// <summary>
        /// Adds to the current scaled-time shake magnitude and extends the duration if needed.
        /// </summary>
        /// <param name="magnitude">The shake magnitude to add.</param>
        /// <param name="duration">The minimum resulting shake duration in seconds.</param>
        public void ShakeAdditive(float magnitude, float duration)
        {
            this.magnitude += magnitude;
            this.duration = Mathf.Max(this.duration, duration);
            remaining = this.duration;
            useUnscaledTime = false;
        }

        /// <summary>
        /// Adds to the current unscaled-time shake magnitude and extends the duration if needed.
        /// </summary>
        /// <param name="magnitude">The shake magnitude to add.</param>
        /// <param name="duration">The minimum resulting shake duration in seconds.</param>
        public void ShakeAdditiveUnscaled(float magnitude, float duration)
        {
            this.magnitude += magnitude;
            this.duration = Mathf.Max(this.duration, duration);
            remaining = this.duration;
            useUnscaledTime = true;
        }
    }
}
#endif
