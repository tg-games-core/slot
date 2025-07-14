using System;

namespace Core.Services
{
    public class FireTVAdvertisingService : IAdvertisingService
    {
        public void Init()
        {
            throw new NotImplementedException();
        }

        public void ShowRewarded(string placementName, Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public bool IsRewardedAvailable()
        {
            throw new NotImplementedException();
        }

        public void ShowInterstitial(string placementName)
        {
            throw new NotImplementedException();
        }

        public bool IsInterstitialAvailable()
        {
            throw new NotImplementedException();
        }

        public void ShowBanner()
        {
            throw new NotImplementedException();
        }

        public void HideBanner()
        {
            throw new NotImplementedException();
        }
    }
}