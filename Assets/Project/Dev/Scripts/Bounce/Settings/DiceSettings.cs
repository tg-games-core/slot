using UnityEngine;

namespace Project.Bounce.Settings
{
    [CreateAssetMenu(fileName = "DiceSettings", menuName = "Settings/InGame/DiceSettings", order = 0)]
    public class DiceSettings : ScriptableObject
    {
        [field: SerializeField]
        public float MaxSpeed
        {
            get; private set;
        }

        [field: SerializeField]
        public float RotationSpeed
        {
            get; private set;
        }
        
        [field: SerializeField]
        public float Mass
        {
            get; private set;
        }

        [field: SerializeField]
        public float LinearDrag
        {
            get; private set;
        }
        
        [field: SerializeField]
        public float AngularDrag
        {
            get; private set;
        }
        
        [field: SerializeField]
        public float GravityScale
        {
            get; private set;
        }
        
        [field: SerializeField]
        public bool FreezeRotation
        {
            get; private set;
        }

        [field: SerializeField, Header("Visual configs")]
        public DiceVisualConfig[] VisualConfigs
        {
            get; private set;
        }
    }
}