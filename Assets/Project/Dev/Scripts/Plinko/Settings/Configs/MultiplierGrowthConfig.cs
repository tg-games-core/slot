using System;
using Core;
using UnityEngine;

namespace Project.Plinko.Settings.Configs
{
    [Serializable]
    public class MultiplierGrowthConfig
    {
        [field: SerializeField]
        public Range<int> BounceCountRange
        {
            get; private set;
        }
        
        [field: SerializeField]
        public float MultiplierPerHit
        {
            get; private set;
        }
    }
}