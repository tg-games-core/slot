using System;
using VContainer;

namespace Core.Services
{
    public class DefaultInAppPurchaseService : IInAppPurchaseService
    {
        private InAppSettings _inAppSettings;

        public Action Initialized { get; set; }
        public Action<BundleType> Purchased { get; set; }

        bool IInAppPurchaseService.IsInitialized
        {
            get => true;
        }
        
        [Inject]
        private void Construct(InAppSettings inAppSettings)
        {
            _inAppSettings = inAppSettings;
        }
        
        void IService.Init() { }
        
        void IInAppPurchaseService.InitiatePurchase(string productSku, Action callback)
        {
            callback?.Invoke();
            
            var bundle = _inAppSettings.GetBundle(productSku);

            if (bundle != null)
            {
                ((IInAppPurchaseService)this).Purchased?.Invoke(bundle.BundleType);
            }
        }
        
        Product IInAppPurchaseService.GetProduct(string productSku)
        {
            var bundle = _inAppSettings.GetBundle(productSku);
            Product product = null;

            if (bundle != null)
            {
                product = new Product(bundle.ProductSKU, bundle.ProductSKU, string.Empty, string.Empty, bundle.BaseCost,
                    $"{bundle.BaseCost} USD", "USD");
            }

            return product;
        }
        
        void IInAppPurchaseService.RestorePurchases()
        {
            DebugSafe.Log("RestorePurchases");
        }
    }
}