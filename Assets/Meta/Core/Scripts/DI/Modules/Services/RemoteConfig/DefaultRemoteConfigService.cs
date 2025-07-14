using System;

namespace Core.Services
{
    public class DefaultRemoteConfigService : IRemoteConfigService
    {
        Boolean IRemoteConfigService.IsInterstitialEnabled { get => true; }
        
        int IRemoteConfigService.InterstitialTimerAfterRv { get => 80; }
        int IRemoteConfigService.InterstitialTimer { get => 60; }
        int IRemoteConfigService.FirstInterstitialDelay { get => 120; }
        
        void IService.Init()
        {
            DebugSafe.Log($"IRemoteConfigService: Init");
        }
    }
}