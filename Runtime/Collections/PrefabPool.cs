using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaltboxGames.Unity.Collections
{
    [CreateAssetMenu(menuName = "SaltboxGames/Prefab Pool")]
    public class PrefabPool : ScriptableObject
    {
        [SerializeField]
        private GameObject _prefab;

        [NonSerialized]
        private int count;
        public int ActiveCount => count;
        
        private Stack<PoolableObject> pool = new Stack<PoolableObject>();
        
        private void OnValidate()
        {
            if (_prefab == null)
            {
                return;
            }

            if (!_prefab.TryGetComponent<PoolableObject>(out _))
            {
                Debug.LogError($"{nameof(_prefab)} has no PoolableObject Behaviour", _prefab);
            }
        }
        
        public PoolableObject Spawn(Vector3 position)
        {
            return Spawn(position, Quaternion.identity);
        }

        public PoolableObject Spawn(Vector3 position, Quaternion rotation)
        {
            if (!pool.TryPop(out PoolableObject poolable))
            {
                poolable = CreateInstance(position, rotation);
            }
            else
            {
                poolable.transform.SetPositionAndRotation(position, rotation);
            }

            count++;
            
            poolable.InvokeSpawn();
            poolable.gameObject.SetActive(true);
            return poolable;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Return(PoolableObject poolable)
        {
            if (poolable.Owner != this)
            {
                Debug.LogError($"Trying to return and object that doesn't belong to this pool! {poolable.name}", this);
                return;
            }

            poolable.InvokeDespawned();
            poolable.gameObject.SetActive(false);

            count--;
            
            pool.Push(poolable);
        }

        private PoolableObject CreateInstance(Vector3 position, Quaternion rotation)
        {
            GameObject gameObject = Instantiate(_prefab, position, rotation);
            PoolableObject poolable = gameObject.GetComponent<PoolableObject>();
            
            poolable.Owner = this;
            
            return poolable;
        }
    }

}
