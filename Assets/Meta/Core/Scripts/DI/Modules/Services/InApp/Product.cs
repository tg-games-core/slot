using System;
using JetBrains.Annotations;

namespace Core.Services
{
    /// <summary>
    /// Model representing a Product on the store with all its values and
    /// statuses
    /// </summary>
    [Serializable]
    public class Product
    {
        private readonly string _id;
        /// <summary>
        /// The item ID. Usually the same as its Sku
        /// </summary>
        public string Id { get { return _id; } }

        private readonly string _sku;
        /// <summary>
        /// Item unique identifier on the corresponding store
        /// </summary>
        public string Sku { get { return _sku; } }

        private readonly string _title;
        /// <summary>
        /// Item title
        /// </summary>
        public string Title { get { return _title; } }

        private readonly string _description;
        /// <summary>
        /// Item description
        /// </summary>
        public string Description { get { return _description; } }

        private readonly float _price;

        /// <summary>
        /// Raw product price
        /// </summary>
        public float Price { get { return _price; } }

        private readonly string _priceString;

        /// <summary>
        /// Price string already converted to user's currency
        /// </summary>
        public string PriceString { get { return _priceString; } }

        private readonly string _currencyCode;
        public string CurrencyCode { get { return _currencyCode; } }

        private SubscriptionInfo _subscriptionInfo;

        /// <summary>
        /// Holds all necessary information for a subscription product: if it is active,
        /// cancelled, expired, etc...
        ///
        /// Will be null for non-subscription products
        /// </summary>
        [CanBeNull] public SubscriptionInfo SubscriptionInfo {
            get
            {
                return _subscriptionInfo;
            }
            internal set { _subscriptionInfo = value;} }

        /// <summary>
        /// Determines if this product has already been purchased and it
        /// is active (for example, user did not request a refund)
        /// </summary>
        public bool PurchaseActive { get; internal set; }
        
        public Product(
            string id,
            string sku)
        {
            _id = id;
            _sku = sku;
        }
        
        public Product(
            string id,
            string sku,
            string title,
            string description,
            float price,
            string priceString,
            string currencyCode,
            SubscriptionInfo subscriptionInfo = null)
        {
            _id = id;
            _sku = sku;
            _title = title;
            _description = description;
            _price = price;
            _priceString = priceString;
            _currencyCode = currencyCode;
            _subscriptionInfo = subscriptionInfo;
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Product objAsProduct = obj as Product;
            if (objAsProduct == null) return false;
            else return objAsProduct.Sku == _sku;
        }

        public override int GetHashCode()
        {
            return Sku.GetHashCode();
        }
    }
}