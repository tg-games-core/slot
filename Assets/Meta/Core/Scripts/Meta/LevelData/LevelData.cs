using System;
using VContainer;
using VContainer.Unity;

namespace Core.Data
{
    public class LevelData : StorageObject<LevelStorageData>, ILevelData, IInitializable, IDisposable
    {
        [Inject]
        public LevelData(Storage storage) : base(storage) { }

        void ILevelData.Reset() { }

        void IInitializable.Initialize()
        {
            Load();
        }

        public void Dispose()
        {
            Save();
        }
    }
}