using System;

namespace Core.Services
{
    public interface IAdvertisingService : IService
    {
        void ShowRewarded(string placementName, Action<bool> callback);
        bool IsRewardedAvailable();
        void ShowInterstitial(string placementName);
        bool IsInterstitialAvailable();
        void ShowBanner();
        void HideBanner();
    }
}