using System;
using Core.Data;

namespace Core
{
    public static class CurrencyHelper
    {
        public static CurrencyType GetCurrencyType(this RewardType rewardType) => rewardType switch
        {
            RewardType.Money => CurrencyType.Money,
            _ => throw new Exception($"Not found {nameof(RewardType)} - {rewardType}")
        };

        public static RewardType GetRewardType(this CurrencyType currencyType) => currencyType switch
        {
            CurrencyType.Money => RewardType.Money,
            _ => throw new Exception($"Not found {nameof(CurrencyType)} - {currencyType}")
        };
    }
}