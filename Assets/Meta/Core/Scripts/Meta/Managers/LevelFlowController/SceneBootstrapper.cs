using Core.Services;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class SceneBootstrapper : IInitializable
    {
        private const int AudioSourceCount = 10;
        
        private IPoolSystem _poolSystem;
        private PoolSettings _poolSettings;

        [Inject]
        private void Construct(IPoolSystem poolSystem, PoolSettings poolSettings)
        {
            _poolSystem = poolSystem;
            _poolSettings = poolSettings;
        }
        
        void IInitializable.Initialize()
        {
            _poolSystem.ReleaseAll();

            InitializePool();
        }

        private void InitializePool()
        {
            _poolSystem.CreatePool(_poolSettings.PooledAudio, PooledObjectType.Save, AudioSourceCount);
        }
    }
}