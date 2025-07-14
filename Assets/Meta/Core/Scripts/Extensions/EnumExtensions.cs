using System;

namespace Core
{
    public static class EnumExtensions
    {
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException(string.Format("Argument {0} is not an Enum", typeof(T).FullName));
            }

            T[] arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(arr, src) + 1;
            return (arr.Length==j) ? arr[0] : arr[j];            
        }
        
        public static T Prev<T>(this T src) where T : struct
        {
            T[] arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf(arr, src) - 1;
            return j == -1 ? arr[^1] : arr[j];
        }
    }
}