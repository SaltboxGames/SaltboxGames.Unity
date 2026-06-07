// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2024-2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System.Collections.Generic;
using SaltboxGames.Core.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaltboxGames.Unity.Extensions
{
    /// <summary>
    /// Provides extension methods for Unity scenes.
    /// </summary>
    public static class SceneExtensions
    {
        /// <summary>
        /// Attempts to find a component of the requested type on one of the scene's root game objects.
        /// </summary>
        /// <typeparam name="T">The component type to search for.</typeparam>
        /// <param name="scene">The scene whose root game objects are searched.</param>
        /// <param name="component">The found component, or <see langword="default"/> when no root component matches.</param>
        /// <returns><see langword="true"/> when a matching component is found; otherwise, <see langword="false"/>.</returns>
        public static bool TryGetRootComponent<T>(this Scene scene, out T component)
            where T : Component
        {
            List<GameObject> gameObjects = ListPool<GameObject>.Shared.Rent();
            scene.GetRootGameObjects(gameObjects);

            try
            {
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    if (gameObjects[i].TryGetComponent<T>(out component))
                    {
                        return true;
                    }
                }
            }
            finally
            {
                ListPool<GameObject>.Shared.Return(gameObjects);
            }
            
            component = default;
            return false;
        }
    }
}
