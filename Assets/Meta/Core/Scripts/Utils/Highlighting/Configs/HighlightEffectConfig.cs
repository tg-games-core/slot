using System;
using UnityEngine;

namespace Core.Highlighting
{
    [Serializable]
    public abstract class HighlightEffectConfig
    {
        [field: SerializeField, Header("In")]
        public float InDuration
        {
            get; private set;
        }

        [field: SerializeField]
        public AnimationCurve InCurve
        {
            get; private set;
        }

        [field: SerializeField, Header("Out")]
        public float OutDuration
        {
            get; private set;
        }
        
        [field: SerializeField]
        public AnimationCurve OutCurve
        {
            get; private set;
        }
    }
}