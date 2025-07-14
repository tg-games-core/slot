using System;
using R3;

namespace Core.Data
{
    public interface IUser
    {
        public Action<CurrencyType, int> CurrencyChanged
        {
            get;
            set;
        }
        
        bool CanSpend(CurrencyType type, int count);
        void Spend(CurrencyType type, int count);
        void ClaimReward(RewardItem[] rewards);
        void ClaimReward(RewardItem reward);
        
        ReactiveProperty<int> GetCurrencyProperty(CurrencyType currencyType);
    }
}