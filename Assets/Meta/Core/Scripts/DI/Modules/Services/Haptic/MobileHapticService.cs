namespace Core.Services
{
    public class MobileHapticService : IHapticService
    {
        bool IHapticService.IsHapticEnabled
        {
            get => LocalConfig.IsHapticEnabled;
            set => LocalConfig.IsHapticEnabled = value;
        }

        void IService.Init() { }

        void IHapticService.ToggleHaptic(bool isEnabled)
        {
            ((IHapticService)this).IsHapticEnabled = isEnabled;
        }

        void IHapticService.PlayEmphasis(float amplitude, float frequency)
        {
            if (((IHapticService)this).IsHapticEnabled)
            {
            }
        }

        void IHapticService.PlayConstant(float amplitude, float frequency, float duration)
        {
            if (((IHapticService)this).IsHapticEnabled)
            {
            }
        }
    }
}