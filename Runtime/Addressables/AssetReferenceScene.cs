// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2024-2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

#if ADDRESSABLES_2

using System;
using Cysharp.Threading.Tasks;
using SaltboxGames.Core.Shims;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace SaltboxGames.Unity
{
    /// <summary>
    /// Serializable scene reference backed by a <see cref="SafeGuid"/> for Addressables scene loading.
    /// </summary>
    [Serializable]
    public class AssetReferenceScene
    {
        /// <summary>
        /// Serialized Unity asset GUID for the referenced scene.
        /// </summary>
        [SerializeField, HideInInspector]
        public SafeGuid _sceneGuid;

        /// <summary>
        /// Loads the referenced scene through Addressables.
        /// </summary>
        /// <param name="mode">The scene load mode.</param>
        /// <returns>A task that resolves to the loaded scene instance.</returns>
        public async UniTask<SceneInstance> LoadSceneAsync(LoadSceneMode mode = LoadSceneMode.Additive)
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(_sceneGuid, mode);
            if (handle.IsDone)
            {
                return handle.Result;
            }
            
            return await handle.Task;
        }
    }
}

#endif
