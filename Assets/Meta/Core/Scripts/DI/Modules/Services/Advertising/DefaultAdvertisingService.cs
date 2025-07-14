using System;
using Core.Data;
using UnityEngine;
using VContainer;

namespace Core.Services
{
    public class DefaultAdvertisingService : IAdvertisingService
    {
        private float _nextInterstitialTime;
        
        private OverlayNotificationSystem _overlayNotificationSystem;
        private IRemoteConfigService _remoteConfigService;

        [Inject]
        public DefaultAdvertisingService(OverlayNotificationSystem overlayNotificationSystem, IRemoteConfigService remoteConfigService)
        {
            _remoteConfigService = remoteConfigService;
            _overlayNotificationSystem = overlayNotificationSystem;
        }

        void IService.Init()
        {
            _nextInterstitialTime = Time.realtimeSinceStartup + _remoteConfigService.FirstInterstitialDelay;
        }

        void IAdvertisingService.ShowRewarded(string placementName, Action<bool> callback)
        {
            if (((IAdvertisingService)this).IsRewardedAvailable())
            {
                callback?.Invoke(true);
                
                if (_nextInterstitialTime <
                    Time.realtimeSinceStartup + _remoteConfigService.InterstitialTimerAfterRv)
                {
                    _nextInterstitialTime =
                        Time.realtimeSinceStartup + _remoteConfigService.InterstitialTimerAfterRv;
                }
                
                DebugSafe.Log($"ShowRewarded. Place - {placementName}.");
            }
            else
            {
                _overlayNotificationSystem.Show($"NO INTERNET CONNECTION");
            }
        }

        bool IAdvertisingService.IsRewardedAvailable()
        {
            return true;
        }

        void IAdvertisingService.ShowInterstitial(string placementName)
        {
            if (((IAdvertisingService)this).IsInterstitialAvailable())
            {
                _nextInterstitialTime = Time.realtimeSinceStartup + _remoteConfigService.InterstitialTimer;
                
                DebugSafe.Log($"ShowInterstitial. Place - {placementName}.");
            }
        }

        bool IAdvertisingService.IsInterstitialAvailable()
        {
            return _remoteConfigService.IsInterstitialEnabled && _nextInterstitialTime <= Time.realtimeSinceStartup;
        }

        void IAdvertisingService.ShowBanner()
        {
            DebugSafe.Log($"Show Banner.");
        }

        void IAdvertisingService.HideBanner()
        {
            DebugSafe.Log($"Hide Banner.");
        }
    }
}