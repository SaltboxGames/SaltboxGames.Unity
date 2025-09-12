/*
 * Copyright (c) 2024 SaltboxGames, Jonathan Gardner
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
    public static class SceneExtensions
    {
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
        } }
}
