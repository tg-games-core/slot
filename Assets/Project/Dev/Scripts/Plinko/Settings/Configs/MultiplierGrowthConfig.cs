using System;
using Core;
using Project.Plinko.Settings.Configs.Type;
using UnityEngine;

namespace Project.Plinko.Settings.Configs
{
    [Serializable]
    public class MultiplierGrowthConfig
    {
        [field: SerializeField]
        public GrowthType GrowthType
        {
            get; private set;
        }
        
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