using System;
using JetBrains.Annotations;

namespace Core.Services
{
    public class SubscriptionInfo
    {
        /// <summary>
        /// If the subscription is currently active
        /// </summary>
        public bool IsSubscribed;
 
        /// <summary>
        /// If the subscription has expired
        /// </summary>
        public bool IsExpired;
 
        /// <summary>
        /// If the subscription is cancelled
        /// </summary>
        public bool IsCancelled;
 
        /// <summary>
        /// If the subscription is currently under free trial period
        /// </summary>
        public bool IsFreeTrial;
 
        /// <summary>
        /// If the subscription will be renewed automatically
        /// </summary>
        public bool IsAutoRenewing;
 
        /// <summary>
        /// Time until the subscription becomes inactive or it is auto-renewed
        /// </summary>
        [CanBeNull] public TimeSpan? RemainingTime;
 
        [CanBeNull] public DateTime? PurchaseDate;
 
        [CanBeNull] public DateTime? ExpireDate;
 
        public bool IsIntroductoryPricePeriod;
 
        [CanBeNull] public TimeSpan? IntroductoryPricePeriod;
 
        public long IntroductoryPricePeriodCycles;
 
        public string IntroductoryPrice;
    }
}