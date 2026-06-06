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
using System.Reflection;
using System.Threading.Tasks;
using SaltboxGames.Core.Services;
using UnityEngine;

namespace SaltboxGames.Unity.Services
{
    /// <summary>
    /// Unity component wrapper around <see cref="ServiceScope"/> that registers child <see cref="MonoService"/> components.
    /// </summary>
    public sealed class MonoServiceScope : MonoBehaviour, IServiceScope
    {
        private IServiceScope _internal;
        private IServiceScope Scope
        {
            get
            {
                _internal ??= CreateScope();
                return _internal;
            }
        }
        
        IServiceScope IServiceScope.Parent
        {
            get => Scope.Parent;
            set => Scope.Parent = value;
        }
        
        private List<MonoService> services = new List<MonoService>();
        
        private IServiceScope CreateScope()
        {
            IServiceScope createdScope = new ServiceScope(name);
                
            GetComponentsInChildren(true, services);
            services.Sort((a, b) => GetServicePriority(a.GetType()).CompareTo(GetServicePriority(b.GetType())));
                    
            foreach (MonoService service in services)
            {
                service.gameObject.SetActive(false);
                createdScope.Register((IService)service);
            }
                
            return createdScope;
        }

        /// <inheritdoc />
        public T GetService<T>() where T : class, IService
        {
            return Scope.GetService<T>();
        }

        /// <inheritdoc />
        public bool TryGetService<T>(out T value) where T : class, IService
        {
            return Scope.TryGetService(out value);
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public ServiceScopeState State => Scope.State;

        /// <inheritdoc />
        public async Task InitializeAsync()
        {
            if(State == ServiceScopeState.Initialized || State == ServiceScopeState.Started)
            {
                return;
            }
            
            await Scope.InitializeAsync();
        }

        /// <inheritdoc />
        public async Task StartServices()
        {
            if(State == ServiceScopeState.Started)
            {
                return;
            }

            await Scope.StartServices();
            await SetMonoServicesEnabled(true);
        }

        /// <inheritdoc />
        public async Task StopServices()
        {
            if(State == ServiceScopeState.Stopped || State == ServiceScopeState.Shutdown)
            {
                return;
            }

            foreach(MonoService service in services)
            {
                service.gameObject.SetActive(false);
            }
            await Scope.StopServices();
        }

        /// <inheritdoc />
        public async Task ShutdownAsync()
        {
            if(State == ServiceScopeState.Shutdown)
            {
                return;
            }
            
            await Scope.ShutdownAsync();
        }

        private async Awaitable SetMonoServicesEnabled(bool enabled)
        {
            await Awaitable.NextFrameAsync(); // [Jon] I promise unity is a good engine.
            foreach(MonoService service in services)
            {
                service.gameObject.SetActive(enabled);
            }
        }

        /// <inheritdoc />
        public void Register<TService>(TService service)
            where TService : class, IService
        {
            if (service is MonoService monoService)
            {
                RegisterMonoService(monoService);
            }

            if (typeof(TService) == typeof(MonoService))
            {
                Scope.Register((IService)service);
                return;
            }

            Scope.Register(service);
        }

        /// <inheritdoc />
        public void Register(IService service)
        {
            if (service is MonoService monoService)
            {
                RegisterMonoService(monoService);
            }

            Scope.Register(service);
        }

        /// <inheritdoc />
        public void AttachScope(IServiceScope child)
        {
            Scope.AttachScope(child);
        }

        void IServiceScope.RemoveChild(IServiceScope child)
        {
            Scope.RemoveChild(child);
        }

        bool IServiceScope.TryFindService<T>(out ServiceEntry service)
        {
            return Scope.TryFindService<T>(out service);
        }

        private static int GetServicePriority(Type type)
        {
            DefaultExecutionOrder attribute = type.GetCustomAttribute<DefaultExecutionOrder>();
            if (attribute == null)
            {
                return 0;
            }

            return attribute.order;
        }

        private void RegisterMonoService(MonoService monoService)
        {
            monoService.enabled = false;
            if (!services.Contains(monoService))
            {
                services.Add(monoService);
            }
        }
    }
}
