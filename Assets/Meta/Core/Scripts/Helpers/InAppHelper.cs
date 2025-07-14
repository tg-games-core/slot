using System;

namespace Core
{
    public static class InAppHelper
    {
        public static bool IsInAppEnabled
        {
            get
            {
                bool isEnabled = false;

#if INAPP_ENABLED
                isEnabled = true;
#endif
                return isEnabled;
            }
        }
        
        public static bool HasSubscription
        {
            get { return (DateTime.Now - LocalConfig.SubscribeTime).Days < 7; }
        }

        public static void UpdateSubscription(DateTime subscribeTime)
        {
            LocalConfig.SubscribeTime = subscribeTime;
        }
    }
}