using System;
using Core.Data;
using UnityEngine;
#if INAPP_ENABLED
using UnityEngine.Purchasing;
#endif

namespace Core.Services
{
    [Serializable]
    public class BundlePackConfig
    {
        [field: SerializeField]
        public string ProductSKU { get; private set; }
        
#if INAPP_ENABLED
        [SerializeField]
        private ProductType _productType;
#endif
#if Mamboo
        [SerializeField, EnabledIf(nameof(_productType), (int)ProductType.Subscription)]
        private string _subscriptionProductSKU;
#endif

        [field: SerializeField]
        public float BaseCost { get; private set; }

        [field: SerializeField]
        public BundleType BundleType { get; private set; }

        [field: SerializeField]
        public CurrencyType CurrencyType { get; private set; }

        [field: SerializeField]
        public string OfferName { get; private set; }

        [field: SerializeField]
        public float DiscountPercent { get; private set; }

        [field: SerializeField]
        public int Duration { get; private set; }

        [field: SerializeField]
        public RewardItem[] RewardItems { get; private set; }
        
#if INAPP_ENABLED
        public ProductType ProductType
        {
            get => _productType;
        }
#endif
#if Mamboo
        public string SubscriptionProductSKU
        {
            get => _subscriptionProductSKU;
        }
#endif

        public BundlePackConfig(string productSKU, float baseCost, BundleType bundleType, CurrencyType currencyType,
            string offerName, float discountPercent, int duration, RewardItem[] rewardItems)
        {
            ProductSKU = productSKU;
            BaseCost = baseCost;
            BundleType = bundleType;
            CurrencyType = currencyType;
            OfferName = offerName;
            DiscountPercent = discountPercent;
            Duration = duration;
            RewardItems = rewardItems;
        }
    }
}