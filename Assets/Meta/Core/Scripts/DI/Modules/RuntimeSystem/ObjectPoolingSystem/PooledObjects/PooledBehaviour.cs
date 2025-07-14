using System.Collections;
using UnityEngine;

namespace Core
{
    public abstract class PooledBehaviour : MonoBehaviour
    {
        [SerializeField]
        private PooledObjectType _pooledObjectType;

        [SerializeField]
        private float _freeTimeout;

        private Transform _defaultParent;
        
        public PooledObjectType PooledObjectType
        {
            get => _pooledObjectType;
        }

        public bool IsFree
        {
            get;
            protected set;
        }

        public float FreeTimeout
        {
            get => _freeTimeout;
            
            protected set
            {
                _freeTimeout = value;
                
                if (_freeTimeout > 0)
                {
                    StartCoroutine(FreeCor());
                }
            }
        }

        public virtual void Prepare(Transform defaultParent, PooledObjectType pooledType)
        {
            _pooledObjectType = pooledType;
            _defaultParent = defaultParent;
        }

        public virtual void SpawnFromPool()
        {
            gameObject.SetActive(true);
            IsFree = false;

            if (_freeTimeout > 0)
            {
                StartCoroutine(FreeCor());
            }
        }

        protected virtual void BeforeReturnToPool()
        {
            transform.SetParent(_defaultParent);
        }

        protected virtual void ReturnToPool()
        {
            gameObject.SetActive(false);
        }

        public virtual void Init()
        {

        }

        public void Free()
        {
            BeforeReturnToPool();
            ReturnToPool();
            
            IsFree = true;
        }

        private IEnumerator FreeCor()
        {
            yield return new WaitForSeconds(_freeTimeout);

            Free();
        }
    }
}