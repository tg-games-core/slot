using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "ShadowSettings", menuName = "Settings/Core/ShadowSettings", order = 0)]
    public class ShadowSettings : ScriptableObject
    {
        [field: SerializeField]
        public float ShadowDepthBias
        {
            get; private set;
        }

        [field: SerializeField]
        public float ShadowDistance
        {
            get; private set;
        }
    }
}