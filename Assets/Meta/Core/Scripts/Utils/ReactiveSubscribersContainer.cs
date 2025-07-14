using System;
using System.Collections.Generic;
using System.Linq;
using R3;

namespace Core
{
    public class ReactiveSubscribersContainer : IDisposable
    {
        private readonly Dictionary<object, List<IDisposable>> _subscribeMap = new();

        public void Subscribe<T>(Observable<T> reactProp, Action<T> callback, object subscriptionKey = null)
        {
            FixSubscriptionKey(ref subscriptionKey);

            if (!_subscribeMap.TryGetValue(subscriptionKey, out var subscribers))
            {
                subscribers = new List<IDisposable>();
                _subscribeMap[subscriptionKey] = subscribers;
            }

            var res = reactProp.Subscribe(callback);

            subscribers.Add(res);
        }

        public void Dispose()
        {
            foreach (var kvp in _subscribeMap)
            {
                foreach (var subscriber in kvp.Value)
                {
                    subscriber.Dispose();
                }

                kvp.Value.Clear();
            }

            _subscribeMap.Clear();
        }

        public void FreeSubscribers(object subscriptionKey = null)
        {
            FixSubscriptionKey(ref subscriptionKey);

            if (!_subscribeMap.TryGetValue(subscriptionKey, out var subscribers))
            {
                return;
            }

            foreach (var subscriber in subscribers)
            {
                subscriber.Dispose();
            }

            subscribers.Clear();
        }

        public bool IsEmpty()
        {
            if (_subscribeMap.Count == 0)
            {
                return true;
            }

            if (_subscribeMap.All(kvp => kvp.Value.Count == 0))
            {
                return true;
            }

            return false;
        }

        private void FixSubscriptionKey(ref object subscriptionKey)
        {
            if (subscriptionKey == null)
            {
                subscriptionKey = this;
            }
        }
    }
}