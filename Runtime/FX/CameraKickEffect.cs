/*
 * Copyright (c) 2024 SaltboxGames, Jonathan Gardner
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */


using Unity.Cinemachine;
using UnityEngine;

namespace CameraTricks
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class CameraKickEffect : CinemachineExtension
    {
        [SerializeField]
        private AnimationCurve _decay = AnimationCurve.EaseInOut(0, 1, 1, 0);
        
        private Vector3 kickOffset;
        private float duration;
        private float remaining;
        
        private bool useUnscaledTime;

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

        public void Kick(Vector3 direction, float strength, float duration)
        {
            kickOffset = direction.normalized * strength;
            this.duration = duration;
            remaining = duration;
            useUnscaledTime = false;
        }

        public void KickUnscaled(Vector3 direction, float strength, float duration)
        {
            kickOffset = direction.normalized * strength;
            this.duration = duration;
            remaining = duration;
            useUnscaledTime = true;
        }

        public void SetDecayCurve(AnimationCurve curve)
        {
            _decay = curve ?? AnimationCurve.EaseInOut(0, 1, 1, 0);
        }
    }

}