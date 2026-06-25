// SPDX-License-Identifier: MPL-2.0
/*
 * Copyright (c) 2026 Saltbox Games Cooperative
 *
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System;
using SaltboxGames.Core.Services;
using UnityEngine;

namespace SaltboxGames.Unity.Services
{
    public interface IServiceScopeProvider
    {
        IServiceScope Services { get; }
    }

    public static class ServiceScopeProviderExtensions
    {
        public static IServiceScope GetServiceScope(this Component component)
        {
            IServiceScopeProvider provider = component.GetComponentInParent<IServiceScopeProvider>();
            if (provider != null)
            {
                return provider.Services;
            }
            
            throw new InvalidOperationException($"{component.name} could not find an {nameof(IServiceScopeProvider)} in its parent hierarchy.");
        }
        
        public static T GetService<T>(this Component component)
            where T : class, IService
        {
            IServiceScope serviceScope = GetServiceScope(component);
            return serviceScope.GetService<T>();
        }
        
        public static T GetService<T>(this Component component, ref T service)
            where T : class, IService
        {
            if(service != null)
            {
                return service;
            }
            
            IServiceScope serviceScope = GetServiceScope(component);
            service = serviceScope.GetService<T>();
            return service;
        }
    }
}
