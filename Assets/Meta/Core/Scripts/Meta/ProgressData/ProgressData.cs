using System;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Data
{
    public class ProgressData : StorageObject<ProgressDataStorageData>, IProgressData, IInitializable, IDisposable
    {
        private readonly ReactiveProperty<int> _levelIndex = new(0);

        [Inject]
        public ProgressData(Storage storage) : base(storage) { }

        ReadOnlyReactiveProperty<int> IProgressData.LevelIndex
        {
            get => _levelIndex;
        }

        void IInitializable.Initialize()
        {
            Load();
            
            _levelIndex.Value = ConcreteData.LevelIndex;

            SubscribeOnProgressDataChanges();
        }

        void IProgressData.SetLevelIndex(int levelIndex)
        {
            _levelIndex.Value = Mathf.Max(0, levelIndex);
        }

        void IDisposable.Dispose()
        {
            Save();
        }
        
        private void SubscribeOnProgressDataChanges()
        {
            IProgressData progressData = this;
            
            _reactiveContainer.Subscribe(progressData.LevelIndex, index =>
            {
                ConcreteData.LevelIndex = index;
                
                Save();
            });
        }
    }
}