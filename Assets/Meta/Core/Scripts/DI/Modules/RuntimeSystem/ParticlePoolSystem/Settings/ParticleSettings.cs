using System;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "ParticleSettings", menuName = "Settings/Core/ParticleSettings", order = 0)]
    public class ParticleSettings : ScriptableObject
    {
        [Serializable]
        public class ParticlePreset
        {
            [field: SerializeField]
            public PooledParticle Particle
            {
                get;
                private set;
            }

            [field: SerializeField]
            public ParticleType Type
            {
                get;
                private set;
            }
        }   
        
        [field: SerializeField]
        public ParticlePreset[] ParticlePresets
        {
            get;
            private set;
        }
    }
}