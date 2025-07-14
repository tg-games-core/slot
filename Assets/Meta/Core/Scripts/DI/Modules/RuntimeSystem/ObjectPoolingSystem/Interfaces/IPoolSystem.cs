using UnityEngine;

namespace Core
{
    public interface IPoolSystem
    {
        void CreatePool(PooledBehaviour prefab, PooledObjectType pooledType, int initialCount);
        
        T Get<T>(PooledBehaviour prefab, Vector3 position, Quaternion rotation, Transform parent = null)
            where T : PooledBehaviour;

        void ReleaseAll();
    }
}