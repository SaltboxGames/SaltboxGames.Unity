// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaltboxGames.Core.Services;
using SaltboxGames.Unity.Utilities;
using UnityEngine;

#if ENABLE_PROFILER
using System.Reflection;
using Unity.Profiling;
#endif

namespace SaltboxGames.Unity.Services
{
    /// <summary>
    /// Service that exposes Unity player-loop phases as disposable callback subscriptions.
    /// </summary>
    public sealed class PlayerLoopService : IService
    {
        private readonly List<Subscription> earlyUpdateCallbacks = new List<Subscription>();
        private readonly List<Subscription> updateCallbacks = new List<Subscription>();
        private readonly List<Subscription> lateUpdateCallbacks = new List<Subscription>();

        /// <inheritdoc />
        public Task InitializeAsync(IServiceInitializer services)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void StartService()
        {
            PlayerLoopUtil.InsertIntoEarlyUpdate<PlayerLoopService>(EarlyUpdate);
            PlayerLoopUtil.InsertIntoUpdate<PlayerLoopService>(Update);
            PlayerLoopUtil.InsertIntoLateUpdate<PlayerLoopService>(LateUpdate);
        }

        /// <summary>
        /// Subscribes a callback to the EarlyUpdate phase.
        /// </summary>
        /// <param name="callback">The callback to invoke each EarlyUpdate.</param>
        /// <returns>A disposable subscription that stops invoking the callback when disposed.</returns>
        public IDisposable SubscribeEarlyUpdate(Action callback)
        {
            Subscription subscription = new Subscription(callback, nameof(EarlyUpdate));
            earlyUpdateCallbacks.Add(subscription);
            return subscription;
        }

        /// <summary>
        /// Subscribes a callback to the Update phase.
        /// </summary>
        /// <param name="callback">The callback to invoke each Update.</param>
        /// <returns>A disposable subscription that stops invoking the callback when disposed.</returns>
        public IDisposable SubscribeUpdate(Action callback)
        {
            Subscription subscription = new Subscription(callback, nameof(Update));
            updateCallbacks.Add(subscription);
            return subscription;
        }

        /// <summary>
        /// Subscribes a callback to the LateUpdate phase.
        /// </summary>
        /// <param name="callback">The callback to invoke each LateUpdate.</param>
        /// <returns>A disposable subscription that stops invoking the callback when disposed.</returns>
        public IDisposable SubscribeLateUpdate(Action callback)
        {
            Subscription subscription = new Subscription(callback, nameof(LateUpdate));
            lateUpdateCallbacks.Add(subscription);
            return subscription;
        }

        private void EarlyUpdate()
        {
            Invoke(earlyUpdateCallbacks);
        }

        private void Update()
        {
            Invoke(updateCallbacks);
        }

        private void LateUpdate()
        {
            Invoke(lateUpdateCallbacks);
        }

        private static void Invoke(List<Subscription> callbacks)
        {
            // [Jon] Capture count 
            // so we don't call added items in the same frame;
            int count = callbacks.Count;
            for (int i = 0; i < count; i++)
            {
                Subscription subscription = callbacks[i];
                if (!subscription.IsDisposed)
                {
#if ENABLE_PROFILER
                    subscription.ProfilerMarker.Begin();
#endif
                    try
                    {
                        subscription.Callback.Invoke();
                    }
                    catch (Exception exception)
                    {
                        Debug.LogException(exception);
                    }
                    finally
                    {
#if ENABLE_PROFILER
                        subscription.ProfilerMarker.End();
#endif
                    }
                }
            }

            callbacks.RemoveAll(static s => s.IsDisposed);
        }

        /// <inheritdoc />
        public void StopService()
        {
            PlayerLoopUtil.RemoveFromEarlyUpdate<PlayerLoopService>(EarlyUpdate);
            PlayerLoopUtil.RemoveFromUpdate<PlayerLoopService>(Update);
            PlayerLoopUtil.RemoveFromLateUpdate<PlayerLoopService>(LateUpdate);
        }

        /// <inheritdoc />
        public Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }
        
        private sealed class Subscription : IDisposable
        {
            public readonly Action Callback;
#if ENABLE_PROFILER
            public readonly ProfilerMarker ProfilerMarker;
#endif
            public bool IsDisposed;

            public Subscription(Action callback, string phaseName)
            {
                Callback = callback;
#if ENABLE_PROFILER
                ProfilerMarker = new ProfilerMarker(GetProfilerMarkerName(callback, phaseName));
#endif
            }

            public void Dispose()
            {
                IsDisposed = true;
            }

#if ENABLE_PROFILER
            private static string GetProfilerMarkerName(Action callback, string phaseName)
            {
                MethodInfo method = callback.Method;
                string typeName = method.DeclaringType?.FullName ?? "<unknown>";
                return $"{nameof(PlayerLoopService)}.{phaseName}::{typeName}.{method.Name}";
            }
#endif
        }
    }
}
