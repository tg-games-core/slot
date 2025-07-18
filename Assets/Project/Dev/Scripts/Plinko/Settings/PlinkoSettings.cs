using Project.Plinko.Settings.Configs;
using UnityEngine;

namespace Project.Plinko.Settings
{
    [CreateAssetMenu(fileName = "PlinkoSettings", menuName = "Settings/InGame/PlinkoSettings", order = 0)]
    public class PlinkoSettings : ScriptableObject
    {
        [field: SerializeField]
        public float StartBalance
        {
            get; private set;
        }

        [field: SerializeField]
        public float GameCost
        {
            get; private set;
        }
        
        [field: SerializeField]
        public BounceConfig BounceConfig
        {
            get; private set;
        }
        
        [field: SerializeField]
        public float MultiplierPenaltyOnDiceLoss
        {
            get; private set;
        }
    }
}