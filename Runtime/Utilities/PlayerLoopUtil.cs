// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2024-2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SaltboxGames.Unity.Utilities
{
    /// <summary>
    /// Provides helpers for inserting and removing systems from Unity's player loop.
    /// </summary>
    public class PlayerLoopUtil
    {
        /// <summary>
        /// Inserts an update callback at the start of Unity's EarlyUpdate subsystem list.
        /// </summary>
        /// <typeparam name="T">The marker type assigned to the inserted player-loop system.</typeparam>
        /// <param name="updateFunction">The callback to invoke during EarlyUpdate.</param>
        public static void InsertIntoEarlyUpdate<T>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            PlayerLoopSystem earlyUpdateSystem = new PlayerLoopSystem()
            {
                type = typeof(T),
                updateDelegate = updateFunction,
            };

            InsertInto(typeof(EarlyUpdate), earlyUpdateSystem);
        }

        /// <summary>
        /// Removes matching update callbacks from Unity's EarlyUpdate subsystem list.
        /// </summary>
        /// <typeparam name="T">The marker type used by the caller.</typeparam>
        /// <param name="updateFunction">The callback to remove.</param>
        public static void RemoveFromEarlyUpdate<T>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            RemoveFrom(typeof(EarlyUpdate), updateFunction);
        }

        /// <summary>
        /// Inserts an update callback at the start of Unity's Update subsystem list.
        /// </summary>
        /// <typeparam name="T">The marker type assigned to the inserted player-loop system.</typeparam>
        /// <param name="updateFunction">The callback to invoke during Update.</param>
        public static void InsertIntoUpdate<T>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            PlayerLoopSystem earlyUpdateSystem = new PlayerLoopSystem()
            {
                type = typeof(T),
                updateDelegate = updateFunction,
            };

            InsertInto(typeof(Update), earlyUpdateSystem);
        }

        /// <summary>
        /// Removes matching update callbacks from Unity's Update subsystem list.
        /// </summary>
        /// <typeparam name="T">The marker type used by the caller.</typeparam>
        /// <param name="updateFunction">The callback to remove.</param>
        public static void RemoveFromUpdate<T>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            RemoveFrom(typeof(Update), updateFunction);
        }

        /// <summary>
        /// Inserts an update callback at the start of Unity's PostLateUpdate subsystem list.
        /// </summary>
        /// <typeparam name="T">The marker type assigned to the inserted player-loop system.</typeparam>
        /// <param name="updateFunction">The callback to invoke during PostLateUpdate.</param>
        public static void InsertIntoLateUpdate<T>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            PlayerLoopSystem earlyUpdateSystem = new PlayerLoopSystem()
            {
                type = typeof(T),
                updateDelegate = updateFunction,
            };

            InsertInto(typeof(PostLateUpdate), earlyUpdateSystem);
        }

        /// <summary>
        /// Removes matching update callbacks from Unity's PostLateUpdate subsystem list.
        /// </summary>
        /// <typeparam name="T">The marker type used by the caller.</typeparam>
        /// <param name="updateFunction">The callback to remove.</param>
        public static void RemoveFromLateUpdate<T>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            RemoveFrom(typeof(PostLateUpdate), updateFunction);
        }

        /// <summary>
        /// Inserts a player-loop system at the start of the specified top-level subsystem.
        /// </summary>
        /// <param name="target">The top-level player-loop subsystem type to modify.</param>
        /// <param name="newSystem">The player-loop system to insert.</param>
        public static void InsertInto(Type target, PlayerLoopSystem newSystem)
        {
            PlayerLoopSystem loop = PlayerLoop.GetCurrentPlayerLoop();

            for (int i = 0; i < loop.subSystemList.Length; i++)
            {
                if (loop.subSystemList[i].type != target)
                {
                    continue;
                }
                
                PlayerLoopSystem[] oldList = loop.subSystemList[i].subSystemList;
                PlayerLoopSystem[] newList = new PlayerLoopSystem[oldList.Length + 1];

                newList[0] = newSystem;
                Array.Copy(oldList, 0, newList, 1, oldList.Length);

                loop.subSystemList[i].subSystemList = newList;
                break;
            }

            PlayerLoop.SetPlayerLoop(loop);
        }

        private static void RemoveFrom(
            Type target,
            PlayerLoopSystem.UpdateFunction updateFunction)
        {
            PlayerLoopSystem loop = PlayerLoop.GetCurrentPlayerLoop();

            for (int i = 0; i < loop.subSystemList.Length; i++)
            {
                if (loop.subSystemList[i].type != target)
                {
                    continue;
                }

                var oldList = loop.subSystemList[i].subSystemList;

                int removeCount = 0;

                for (int j = 0; j < oldList.Length; j++)
                {
                    if (oldList[j].updateDelegate == updateFunction)
                    {
                        removeCount++;
                    }
                }

                if (removeCount == 0)
                {
                    return;
                }

                var newList = new PlayerLoopSystem[oldList.Length - removeCount];

                int write = 0;

                for (int j = 0; j < oldList.Length; j++)
                {
                    if (oldList[j].updateDelegate == updateFunction)
                    {
                        continue;
                    }

                    newList[write++] = oldList[j];
                }

                loop.subSystemList[i].subSystemList = newList;
                PlayerLoop.SetPlayerLoop(loop);
                return;
            }
        }

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void ResetLoopInEditor()
        {
            EditorApplication.playModeStateChanged += change =>
            {
                if (change == PlayModeStateChange.ExitingPlayMode)
                {
                    PlayerLoop.SetPlayerLoop(PlayerLoop.GetDefaultPlayerLoop());
                }
            };
        }
#endif
    }
}
