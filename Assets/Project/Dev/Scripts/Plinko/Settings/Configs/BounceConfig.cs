using System;
using System.Linq;
using UnityEngine;

namespace Project.Plinko.Settings.Configs
{
    [Serializable]
    public class BounceConfig
    {
        [SerializeField]
        private MultiplierGrowthConfig[] _configs;

        public MultiplierGrowthConfig GetConfig(int index)
        {
            var config = _configs.FirstOrDefault(c => c.BounceCountRange.IsInRange(index));

            if (config == null)
            {
                Debug.LogError($"Not found {nameof(MultiplierGrowthConfig)} for index - {index}");

                config = _configs[^1];
            }

            return config;
        }
    }
}