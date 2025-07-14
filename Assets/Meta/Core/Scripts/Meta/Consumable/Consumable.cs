using System;
using Core.Services;
using ObservableCollections;
using VContainer;
using VContainer.Unity;

namespace Core.Data
{
    public class Consumable : StorageObject<ConsumableStorageData>, IConsumable, IInitializable, IDisposable
    {
        private ObservableDictionary<BundleType, DateTime> _purchasedShopBundles = new();

        private IConsumable _consumable;

        [Inject]
        public Consumable(Storage storage) : base(storage) { }

        Action<BundleType> IConsumable.Purchased
        {
            get; set;
        }
        
        void IConsumable.Purchase(BundleType bundleType)
        {
            if (!_purchasedShopBundles.ContainsKey(bundleType))
            {
                _purchasedShopBundles.Add(bundleType, DateTime.Now);
            }
            
            _purchasedShopBundles[bundleType] = DateTime.Now;
            
            _consumable.Purchased?.Invoke(bundleType);
        }

        bool IConsumable.IsBundlePurchased(BundleType bundleType)
        {
            return _purchasedShopBundles.ContainsKey(bundleType);
        }

        DateTime IConsumable.GetBundlePurchaseTime(BundleType bundleType)
        {
            if (!_purchasedShopBundles.TryGetValue(bundleType, out var dateTime))
            {
                dateTime = DateTime.MinValue;
            }

            return dateTime;
        }
        
        void IInitializable.Initialize()
        {
            Load();
            
            _consumable = this;

            _purchasedShopBundles = new(ConcreteData.PurchasedShopBundles);
            
            SubscribeOnConsumableDataChanges();
        }

        private void SubscribeOnConsumableDataChanges()
        {
            _reactiveContainer.Subscribe(_purchasedShopBundles.ObserveReplace(), purchase =>
            {
                ConcreteData.PurchasedShopBundles[purchase.NewValue.Key] = purchase.NewValue.Value;
                
                Save();
            });
        }

        void IDisposable.Dispose()
        {
            Save();
        }
    }
}