/*
 * Copyright (c) 2024 SaltboxGames, Jonathan Gardner
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */


using System;
using Cysharp.Threading.Tasks;
using SaltboxGames.Core.Terminals;

namespace SaltboxGames.Unity
{
    public static class CommandRegistryUnityExtensions
    {
        public static void Register<T>(this ICommandRegistry commandRegistry, Func<T, UniTask<string>> handler)
        {
            commandRegistry.Register<T>(com => handler(com).AsValueTask());
        }
    }
}
