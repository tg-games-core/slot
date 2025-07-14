using System;
using R3;
using VContainer;
using VContainer.Unity;

namespace Core.Data
{
    public class User : StorageObject<UserStorageData>, IUser, IInitializable, IDisposable
    {
        private readonly ReactiveProperty<int> _money = new(0);
        
        private IUser _user;

        [Inject]
        public User(Storage storage) : base(storage) { }

        Action<CurrencyType, int> IUser.CurrencyChanged
        {
            get; set;
        }
        
        bool IUser.CanSpend(CurrencyType currencyType, int count)
        {
            bool canSpend;

            switch (currencyType)
            {
                case CurrencyType.Money:
                    canSpend = count <= _user.GetCurrencyProperty(currencyType).Value;
                    break;

                default:
                    DebugSafe.LogError($"CanSpend not implemented for {nameof(CurrencyType)} - {currencyType}");
                    canSpend = false;
                    break;
            }

            return canSpend;
        }

        void IUser.Spend(CurrencyType currencyType, int count)
        {
            var currencyProperty = _user.GetCurrencyProperty(currencyType); 
            currencyProperty.Value -= count;
            
            _user.CurrencyChanged?.Invoke(currencyType, count);
        }

        void IUser.ClaimReward(RewardItem[] rewards)
        {
            for (int i = 0; i < rewards.Length; i++)
            {
                var reward = rewards[i];
                var currencyType = reward.RewardType.GetCurrencyType();
                
                _user.GetCurrencyProperty(currencyType).Value += reward.Count;
                _user.CurrencyChanged?.Invoke(currencyType, reward.Count);
            }
        }
        
        void IUser.ClaimReward(RewardItem reward)
        {
            var currencyType = reward.RewardType.GetCurrencyType();
            
            _user.GetCurrencyProperty(currencyType).Value += reward.Count;
            _user.CurrencyChanged?.Invoke(currencyType, reward.Count);
        }

        ReactiveProperty<int> IUser.GetCurrencyProperty(CurrencyType currencyType) => currencyType switch
        {
            CurrencyType.Money => _money,
            _ => throw new Exception($"Not found {nameof(CurrencyType)} - {currencyType}")
        };

        void IInitializable.Initialize()
        {
            Load();
            
            _user = this;

            _money.Value = ConcreteData.MoneyCount;

            SubscribeOnUserDataChanges();
        }

        void IDisposable.Dispose()
        {
            Save();
        }

        private void SubscribeOnUserDataChanges()
        {
            _reactiveContainer.Subscribe(_user.GetCurrencyProperty(CurrencyType.Money), i =>
            {
                ConcreteData.MoneyCount = i;

                Save();
            });
        }
    }
}