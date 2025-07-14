namespace Core.Services
{
    public interface IHapticService : IService
    {
        bool IsHapticEnabled { get; set; }
        
        void ToggleHaptic(bool isEnabled);
        void PlayEmphasis(float amplitude, float frequency);
        void PlayConstant(float amplitude, float frequency, float duration);
    }
}