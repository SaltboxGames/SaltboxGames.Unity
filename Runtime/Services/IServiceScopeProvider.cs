// using System;
// using System.Collections.Generic;
// using SaltboxGames.Core.Services;
// using UnityEngine;
// using UnityEngine.Pool;

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
            if (serviceScope != null)
            {
                return serviceScope.GetService<T>();
            }
            
            throw new InvalidOperationException($"{component.name} could not find a {nameof(T)} in scope.");
        }
    }
}
