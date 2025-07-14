using System;

namespace Core.Services
{
    public interface IInAppPurchaseService : IService
    {
        public Action Initialized
        {
            get; set;
        }

        public Action<BundleType> Purchased
        {
            get; set;
        }
        
        bool IsInitialized
        {
            get;
        }
        
        void InitiatePurchase(string productSku, Action callback);
        void RestorePurchases();
        Product GetProduct(string productSku);
    }
}