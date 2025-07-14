using System;

namespace Core.Services
{
    public interface IRemoteConfigService : IService
    {
        Boolean IsInterstitialEnabled { get; }

        Int32 InterstitialTimerAfterRv { get; }
        Int32 InterstitialTimer { get; }
        Int32 FirstInterstitialDelay { get; }
    }
}