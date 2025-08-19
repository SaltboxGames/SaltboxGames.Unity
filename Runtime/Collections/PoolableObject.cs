using System;
using UnityEngine;

namespace SaltboxGames.Unity.Collections
{
    public class PoolableObject : MonoBehaviour
    {
        [NonSerialized]
        internal PrefabPool Owner;

        public event Action OnSpawned;
        public event Action OnDespawned;

        internal void InvokeSpawn()
        {
            OnSpawned?.Invoke();
        }

        internal void InvokeDespawned()
        {
            OnDespawned?.Invoke();
        }
        
        public void Despawn()
        {
            Owner.Return(this);
        }
    }
}
