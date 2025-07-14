using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "PoolSettings", menuName = "Settings/Core/PoolSettings", order = 0)]
    public class PoolSettings : ScriptableObject
    {
        [field: SerializeField]
        public PooledAudio PooledAudio
        {
            get;
            private set;
        }
    }
}