/*
 * Copyright (c) 2024 SaltboxGames, Jonathan Gardner
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */


using Unity.Cinemachine;
using UnityEngine;

namespace SaltboxGames.Unity.FX
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class CameraShakeEffect : CinemachineExtension
    {
        private float magnitude;
        private float duration;
        private float remaining;
        private bool useUnscaledTime;
        private System.Random rng = new System.Random();

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

        public void Shake(float magnitude, float duration)
        {
            this.magnitude = Mathf.Max(this.magnitude, magnitude);
            this.duration = Mathf.Max(this.duration, duration);
            remaining = duration;
            useUnscaledTime = false;
        }

        public void ShakeUnscaled(float magnitude, float duration)
        {
            this.magnitude = Mathf.Max(this.magnitude, magnitude);
            this.duration = Mathf.Max(this.duration, duration);
            remaining = duration;
            useUnscaledTime = true;
        }
        
        public void ShakeAdditive(float magnitude, float duration)
        {
            this.magnitude += magnitude;
            this.duration = Mathf.Max(this.duration, duration);
            remaining = this.duration;
            useUnscaledTime = false;
        }

        public void ShakeAdditiveUnscaled(float magnitude, float duration)
        {
            this.magnitude += magnitude;
            this.duration = Mathf.Max(this.duration, duration);
            remaining = this.duration;
            useUnscaledTime = true;
        }
    }
}
