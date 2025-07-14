namespace Core.Wrappers.Animations
{
    public interface IAnimation
    {
        float Duration { get; }

        void Play();
        void Stop();
    }
}