using Core.Interfaces;
using UnityEngine;

namespace Core
{
    public abstract class RuntimeComponent<T> : MonoBehaviour, IRuntimeSystem where T : Component
    {
        public static string Address
        {
            get => $"{typeof(T).Name}";
        }

        public static T GetManager
        {
            get { return Resources.Load<T>($"DIManagers/{typeof(T).Name}"); }
        }


        protected virtual void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public virtual void Initialize()
        {
            gameObject.name = typeof(T).Name;
        }
        public virtual void Dispose() { }
    }
}