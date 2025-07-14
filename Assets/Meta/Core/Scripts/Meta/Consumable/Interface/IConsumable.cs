using System;
using Core.Services;

namespace Core.Data
{
    public interface IConsumable
    {
        Action<BundleType> Purchased
        {
            get; set;
        }
        
        void Purchase(BundleType bundleType);
        bool IsBundlePurchased(BundleType bundleType);
        DateTime GetBundlePurchaseTime(BundleType bundleType);
    }
}