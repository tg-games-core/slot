using UnityEngine;

namespace Project.Bounce.Settings
{
    [CreateAssetMenu(fileName = "DiceSpawnSettings", menuName = "Settings/InGame/DiceSpawnSettings", order = 0)]
    public class DiceSpawnSettings : ScriptableObject
    {
        [SerializeField]
        public PlinkoDice[] _dices;

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

        public PlinkoDice GetDice(int index)
        {
            return _dices[index % _dices.Length];
        }
    }
}