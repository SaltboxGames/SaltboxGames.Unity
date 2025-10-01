/*
 * Copyright (c) 2024 SaltboxGames, Jonathan Gardner
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace SaltboxGames.Unity.Utilities
{
    public class PlayerLoopUtil
    {
        public static void InsertIntoEarlyUpdate<T>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            
            PlayerLoopSystem earlyUpdateSystem = new PlayerLoopSystem()
            {
                type = typeof(T),
                updateDelegate = updateFunction,
            };
            
            InsertInto(typeof(EarlyUpdate), earlyUpdateSystem);
        }
        
        public static void InsertIntoUpdate<T>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            
            PlayerLoopSystem earlyUpdateSystem = new PlayerLoopSystem()
            {
                type = typeof(T),
                updateDelegate = updateFunction,
            };
            
            InsertInto(typeof(Update), earlyUpdateSystem);
        }
        
        public static void InsertIntoLateUpdate<T>(PlayerLoopSystem.UpdateFunction updateFunction)
        {
            
            PlayerLoopSystem earlyUpdateSystem = new PlayerLoopSystem()
            {
                type = typeof(T),
                updateDelegate = updateFunction,
            };
            
            
            InsertInto(typeof(PostLateUpdate), earlyUpdateSystem);
        }
        
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

    }
}
