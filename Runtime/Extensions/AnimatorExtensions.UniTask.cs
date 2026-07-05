// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2024-2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

#if UNITASK_2

using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SaltboxGames.Unity.Extensions
{
    public static class AnimatorExtensions
    {
        public static async UniTask WaitForStateExit(this Animator animator, int layer, CancellationToken cancellationToken = default)
        {
            await UniTask.Yield(cancellationToken);

            await UniTask.WaitUntil(() =>
            {
                var info = animator.GetCurrentAnimatorStateInfo(layer);
                return !animator.IsInTransition(layer) &&
                       info.normalizedTime >= 1f;
            }, cancellationToken: cancellationToken);
        }
    }
}
#endif
