using System;
using System.Collections.Generic;
using System.Linq;
using Core.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class ObjectPoolingSystem : RuntimeComponent<ObjectPoolingSystem>, IPoolSystem
    {
        private readonly Dictionary<PooledBehaviour, Queue<PooledBehaviour>> _pools = new ();

        private IObjectResolver _objectResolver;
        private IPoolSystem _objectPool;

        [Inject]
        private void Construct(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public override void Initialize()
        {
            base.Initialize();

            _objectPool = this;
        }

        void IPoolSystem.CreatePool(PooledBehaviour prefab, PooledObjectType pooledType, int initialCount)
        {
            if (!_pools.TryGetValue(prefab, out var queue))
            {
                queue = new Queue<PooledBehaviour>();
                _pools[prefab] = queue;
            }

            int existingCount = queue.Count;
            if (existingCount < initialCount)
            {
                int toSpawn = initialCount - existingCount;
                for (int i = 0; i < toSpawn; i++)
                {
                    var instance = InstantiateAndPrepare(prefab, pooledType);
                    queue.Enqueue(instance);
                }

                DebugSafe.Log(
                    $"[ObjectPool] Added {toSpawn} instances to pool {prefab.name} (new size: {initialCount}).");
            }
        }

        T IPoolSystem.Get<T>(PooledBehaviour prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (prefab == null)
            {
                DebugSafe.LogException(new Exception("[ObjectPool] Requested prefab is null."));
            }
            
            EnsurePoolInitialized(prefab);
            var instance = GetOrCreateInstance(prefab);
            InitializeInstance(instance, position, rotation, parent);
            return (T)instance;
        }

        void IPoolSystem.ReleaseAll()
        {
            foreach (var kvp in _pools)
            {
                ReleasePool(kvp.Key);
            }
        }

        private void EnsurePoolInitialized(PooledBehaviour prefab)
        {
            if (!_pools.ContainsKey(prefab))
            {
                _objectPool.CreatePool(prefab, PooledObjectType.FreeOnBattleEnd, 1);
                DebugSafe.LogError(
                    $"[ObjectPool] Pool for {prefab} did not exist. Created default pool with one instance.");
            }
        }

        private PooledBehaviour InstantiateAndPrepare(PooledBehaviour prefab, PooledObjectType pooledType)
        {
            var instance = _objectResolver.Instantiate(prefab, gameObject.transform);
            instance.Prepare(transform, pooledType);
            instance.Init();
            instance.Free();
            return instance;
        }

        private PooledBehaviour GetOrCreateInstance(PooledBehaviour prefab)
        {
            if (!_pools.TryGetValue(prefab, out var queue))
            {
                _objectPool.CreatePool(prefab, PooledObjectType.FreeOnBattleEnd, initialCount: 1);
                queue = _pools[prefab];
            }

            var freeInstance = queue.FirstOrDefault(item => item.IsFree);

            if (freeInstance != null)
            {
                return freeInstance;
            }

            var newInstance = InstantiateAndPrepare(prefab, PooledObjectType.FreeOnBattleEnd);
            queue.Enqueue(newInstance);
            DebugSafe.LogError($"[ObjectPool] No free instances in pool for {prefab.name}. Created a new one.");
            return newInstance;
        }

        private void InitializeInstance(PooledBehaviour instance, Vector3 position, Quaternion rotation,
            Transform parent = null)
        {
            if (parent)
            {
                instance.transform.SetParent(parent);
            }

            instance.transform.SetPositionAndRotation(position, rotation);
            instance.SpawnFromPool();
        }

        private void DestroyPool(PooledBehaviour prefab)
        {
            if (_pools.TryGetValue(prefab, out var queue))
            {
                foreach (var pooledInstance in queue)
                {
                    Destroy(pooledInstance.gameObject);
                }

                _pools.Remove(prefab);
            }
        }

        private void ReleasePool(PooledBehaviour prefab)
        {
            if (_pools.TryGetValue(prefab, out var queue))
            {
                foreach (var pooledInstance in queue)
                {
                    pooledInstance.Free();
                }
            }
        }
    }
}
