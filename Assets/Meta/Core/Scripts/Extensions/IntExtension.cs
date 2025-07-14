namespace Core
{
    public static class IntExtension
    {
        public static int Sum(this int[] target)
        {
            int sum = 0;

            for (int i = 0; i < target.Length; i++)
            {
                sum += target[i];
            }
            
            return sum;
        }
    }
}