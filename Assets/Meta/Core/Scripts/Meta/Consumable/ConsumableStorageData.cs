using System;
using System.Collections.Generic;
using Core.Services;

namespace Core.Data
{
    public class ConsumableStorageData : StorageData
    {
        //For Web GL dictionary serialization do not work
        public Dictionary<BundleType, DateTime> PurchasedShopBundles { get; set; } =
            new Dictionary<BundleType, DateTime>();
    }
}