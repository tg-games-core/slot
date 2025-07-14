namespace Core
{
    public struct AudioParams
    {
        public float Pitch;
        public float Volume;
        public bool IsLooped;
        
        public static AudioParams Default => new AudioParams
        {
            Pitch = 1f,
            Volume = 1f,
            IsLooped = false
        };
    }
}