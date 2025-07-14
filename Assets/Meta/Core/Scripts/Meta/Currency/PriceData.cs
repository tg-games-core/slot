using System;
using Core.Data;
using UnityEngine;
#if !UNITY_WEBGL
using Newtonsoft.Json;
#endif

namespace Core
{
    [Serializable]
    public class PriceData
    {
        [SerializeField]
        private CurrencyType _currencyType;

        [SerializeField, EnabledIf(nameof(_currencyType), new int[] { (int)CurrencyType.Money, (int)CurrencyType.Gem })]
        private float _count;

        [SerializeField, EnabledIf(nameof(_currencyType), (int)CurrencyType.Hard)]
        private string _sku;
        
        public float Count
        {
            get => _count;
        }

        public CurrencyType Type
        {
            get => _currencyType;
        }

        public string Sku
        {
            get => _sku;
        }

        public PriceData(CurrencyType type)
        {
            _currencyType = type;
        }
        
        public PriceData(CurrencyType type, float count)
        {
            _currencyType = type;
            _count = count;
        
            if (_currencyType == CurrencyType.Hard)
            {
                DebugSafe.LogException(new Exception("Constructor not allowed for Hard currency!"));
            }
        }
        
#if !UNITY_WEBGL
        [JsonConstructor]
#endif
        public PriceData(CurrencyType type, float count, string sku)
        {
            _currencyType = type;
            _count = count;
            _sku = sku;
        }
    }
}