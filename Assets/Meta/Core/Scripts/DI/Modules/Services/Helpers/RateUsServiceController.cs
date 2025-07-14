using System;
using VContainer;

namespace Core.Services
{
    public static class RateUsServiceController
    {
        public static Type RegisterService(IContainerBuilder builder)
        {
            var targetStore = PublisherHelper.GetTargetCompany();
            Type type = null;

            switch (targetStore)
            {
                case PublisherType.None:
                case PublisherType.FireTV:
                    type = typeof(DefaultRateUsService);
                    break;
                
                default:
                    DebugSafe.LogException(new Exception(
                        $"Not found {nameof(IRateUsService)} for {nameof(PublisherType)}: {targetStore}"));
                    break;
            }
            
            builder.Register(type, Lifetime.Singleton).AsImplementedInterfaces();

            return typeof(IRateUsService);
        }

    }
}