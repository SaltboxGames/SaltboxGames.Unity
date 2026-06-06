// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System.Threading.Tasks;
using SaltboxGames.Core.Services;
using UnityEngine;

namespace SaltboxGames.Unity.Services
{
    /// <summary>
    /// Base class for Unity <see cref="MonoBehaviour"/> components that participate in the SaltboxGames service lifecycle.
    /// </summary>
    public abstract class MonoService : MonoBehaviour, IService
    {
        /// <inheritdoc />
        public Task InitializeAsync(IServiceInitializer services)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public virtual void StartService()
        {
        }

        /// <inheritdoc />
        public virtual void StopService()
        {
        }

        /// <inheritdoc />
        public virtual Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }
    }
}
