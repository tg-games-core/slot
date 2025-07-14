using System;
using UnityEngine;

namespace Core
{
    public static class FloatExtensions
    {
        public static bool AlmostEquals(this float target, float value)
        {
            return Mathf.Abs(target - value) < Mathf.Epsilon;
        }
        
        public static int RoundArithmeticToInt(this float value)
        {
            return (int)Math.Round(value, MidpointRounding.AwayFromZero);
        }

        public static int RoundArithmeticToInt(this double value)
        {
            return (int)Math.Round(value, MidpointRounding.AwayFromZero);
        }

        public static float RoundArithmeticToFloat(this float value)
        {
            return (float)Math.Round(value, MidpointRounding.AwayFromZero);
        }

        public static int RoundOff(this float value)
        {
            return (int)Math.Round(value / 10.0f, MidpointRounding.AwayFromZero) * 10;
        }
    }
}