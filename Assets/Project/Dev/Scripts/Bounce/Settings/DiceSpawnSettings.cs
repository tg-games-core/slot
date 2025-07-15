using UnityEngine;

namespace Project.Bounce.Settings
{
    [CreateAssetMenu(fileName = "DiceSpawnSettings", menuName = "Settings/InGame/DiceSpawnSettings", order = 0)]
    public class DiceSpawnSettings : ScriptableObject
    {
        [field: SerializeField]
        public PlinkoDice Dice
        {
            get; private set;
        }

        [field: SerializeField, Header("Spawn Settings")]
        public int DiceCount
        {
            get; private set;
        }
        
        [field: SerializeField]
        public float SpawnInterval
        {
            get; private set;
        }
    }
}