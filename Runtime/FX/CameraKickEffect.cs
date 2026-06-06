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
    /// Cinemachine extension that applies a decaying positional camera kick.
    /// </summary>
    [RequireComponent(typeof(CinemachineCamera))]
    public class CameraKickEffect : CinemachineExtension
    {
        [SerializeField]
        private AnimationCurve _decay = AnimationCurve.EaseInOut(0, 1, 1, 0);
        
        private Vector3 kickOffset;
        private float duration;
        private float remaining;

        private bool useUnscaledTime;

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

            float t = 1f - (remaining / duration);
            float decayFactor = _decay.Evaluate(t);
            state.PositionCorrection += kickOffset * decayFactor;

            remaining -= useUnscaledTime ? Time.unscaledDeltaTime : deltaTime;
        }

        /// <summary>
        /// Starts a scaled-time camera kick in the provided direction.
        /// </summary>
        /// <param name="direction">The direction of the camera offset.</param>
        /// <param name="strength">The offset magnitude.</param>
        /// <param name="duration">The kick duration in seconds.</param>
        public void Kick(Vector3 direction, float strength, float duration)
        {
            kickOffset = direction.normalized * strength;
            this.duration = duration;
            remaining = duration;
            useUnscaledTime = false;
        }

        /// <summary>
        /// Starts an unscaled-time camera kick in the provided direction.
        /// </summary>
        /// <param name="direction">The direction of the camera offset.</param>
        /// <param name="strength">The offset magnitude.</param>
        /// <param name="duration">The kick duration in seconds.</param>
        public void KickUnscaled(Vector3 direction, float strength, float duration)
        {
            kickOffset = direction.normalized * strength;
            this.duration = duration;
            remaining = duration;
            useUnscaledTime = true;
        }

        /// <summary>
        /// Sets the decay curve used to fade the kick offset over time.
        /// </summary>
        /// <param name="curve">The decay curve, or <see langword="null"/> to restore the default ease-in-out curve.</param>
        public void SetDecayCurve(AnimationCurve curve)
        {
            _decay = curve ?? AnimationCurve.EaseInOut(0, 1, 1, 0);
        }
    }
}

#endif
