using System;
using VContainer;

namespace Core.Services
{
    public static class AdvertisingServiceController
    {
        public static Type RegisterService(IContainerBuilder builder)
        {
            var targetStore = PublisherHelper.GetTargetCompany();
            Type type = null;
            
            switch (targetStore)
            {
                case PublisherType.None:
                    type = typeof(DefaultAdvertisingService);
                    break;
                
                case PublisherType.FireTV:
                    type = typeof(FireTVAdvertisingService);
                    break;
                
                default:
                    DebugSafe.LogException(new Exception(
                        $"Not found {nameof(IAdvertisingService)} for {nameof(PublisherType)}: {targetStore}"));
                    break;
            }
            
            builder.Register(type, Lifetime.Singleton).AsImplementedInterfaces();

            return typeof(IAdvertisingService);
        }
    }
}