using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "LoadingSettings", menuName = "Settings/Core/LoadingSettings", order = 0)]
    public class LoadingSettings : ScriptableObject
    {
        [field: SerializeField]
        public float LoadingTime
        {
            get;
            private set;
        }

        [field: SerializeField]
        public float FadeTime
        {
            get;
            private set;
        }

        [field: SerializeField]
        public AnimationCurve FadeCurve
        {
            get;
            private set;
        }
    }
}