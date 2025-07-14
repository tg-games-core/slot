using System;
using UnityEngine;
#if !UNITY_WEBGL
using Newtonsoft.Json;
#endif

namespace Core
{
    [Serializable]
    public class RewardItem
    {
        [SerializeField]
        private RewardType _rewardType;

        [SerializeField]
        private bool _isInRange;
        
        [SerializeField, EnabledIf(nameof(_isInRange), false)]
        private int _count;

        [SerializeField, EnabledIf(nameof(_isInRange), true)]
        private Range<int> _countRange;

        public RewardType RewardType
        {
            get => _rewardType;
        }

        public int Count
        {
            get => _isInRange ? _countRange.RandomValue : _count;
        }

        public RewardItem(RewardType rewardType, int count)
        {
            _rewardType = rewardType;
            _isInRange = false;
            _count = count;
        }

        public RewardItem(RewardType rewardType, Range<int> countRange)
        {
            _rewardType = rewardType;
            _isInRange = true;
            _countRange = countRange;
        }

        public RewardItem(RewardItem rewardItem)
        {
            _rewardType = rewardItem._rewardType;
            _isInRange = rewardItem._isInRange;
            _count = rewardItem._count;
            _countRange = rewardItem._countRange;
        }

#if !UNITY_WEBGL
        [JsonConstructor]
#endif
        public RewardItem(RewardType rewardType, bool isInRange, int count, Range<int> countRange)
        {
            _rewardType = rewardType;
            _isInRange = isInRange;
            _count = count;
            _countRange = countRange;
        }
    }
}