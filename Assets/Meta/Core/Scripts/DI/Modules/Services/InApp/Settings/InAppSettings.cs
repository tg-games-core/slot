using System;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.Services
{
    [CreateAssetMenu(fileName = "InAppSettings", menuName = "Settings/Core/InAppSettings", order = 0)]
    public class InAppSettings : ScriptableObject
    {
        [field: SerializeField]
        public BundlePackConfig[] BundlePackConfigs
        {
            get;
            private set;
        }
        
        [field: SerializeField, Space(20)]
        public string FormattedJsonData { get; private set; }

        public BundlePackConfig GetBundle(BundleType bundleType)
        {
            var bundle = BundlePackConfigs.FirstOrDefault(b => b.BundleType == bundleType);

            if (bundle == null)
            {
                DebugSafe.LogException(
                    new Exception($"Not found {nameof(BundlePackConfig)} for {nameof(BundleType)} - {bundleType}"));
            }
            
            return bundle;
        }

        public BundlePackConfig GetBundle(string sku)
        {
            var bundle = BundlePackConfigs.FirstOrDefault(b => b.ProductSKU.Equals(sku));

#if Mamboo && INAPP_ENABLED
            if (bundle == null)
            {
                bundle = BundlePackConfigs.FirstOrDefault(b => b.SubscriptionProductSKU.Equals(sku));

                if (bundle == null)
                {
                    DebugSafe.LogException(
                        new Exception($"Not found {nameof(BundlePackConfig)} for SKU - {sku}"));
                }
            }
#else
            if (bundle == null)
            {
                DebugSafe.LogException(
                    new Exception($"Not found {nameof(BundlePackConfig)} for SKU - {sku}"));
            }
#endif
            
            return bundle;
        }
        
        public void ConvertConfigToJson()
        {
            FormattedJsonData = JsonHelper.Convert(BundlePackConfigs);
        }

        public void ConvertJsonToConfig(string jsonData)
        {
            var configs = JsonHelper.Convert<BundlePackConfig[]>(jsonData);
            
            if (configs != null)
            {
                BundlePackConfigs = configs;
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif
        }
    }
}