using System;

namespace Core
{
    public static class RandomValueGenerator
    {
        public static T GenerateRandom<T>(T min, T max) where T : IComparable
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Int32:
                    return (T)(object)UnityEngine.Random.Range((int)(object)min, (int)(object)max + 1);
                case TypeCode.Single:
                    return (T)(object)UnityEngine.Random.Range((float)(object)min, (float)(object)max);
                
                default:
                    throw new NotSupportedException($"Random generation not supported for type {typeof(T)}");
            }
        }
    }
}