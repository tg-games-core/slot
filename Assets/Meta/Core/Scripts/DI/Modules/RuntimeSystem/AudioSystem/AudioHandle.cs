namespace Core
{
    public struct AudioHandle
    {
        public readonly PooledAudio Instance;
        public readonly float Length;

        public AudioHandle(PooledAudio instance, float length)
        {
            Instance = instance;
            Length = length;
        }
    }
}