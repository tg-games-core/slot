using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class Range<T> where T : IComparable
    {
        [field: SerializeField]
        public T Min
        {
            get; private set;
        }

        [field: SerializeField]
        public T Max
        {
            get; private set;
        }
        
        public T RandomValue
        {
            get => RandomValueGenerator.GenerateRandom(Min, Max);
        }

        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public void SetupMin(T value) => Min = value;

        public void SetupMax(T value) => Max = value;

        public bool IsInRange(T value)
        {
            return value.CompareTo(Min) >= 0 && value.CompareTo(Max) <= 0;
        }
    }
}