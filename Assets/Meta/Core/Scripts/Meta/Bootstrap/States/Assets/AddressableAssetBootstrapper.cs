using System;
using System.Collections.Generic;
#if UNITY_WEBGL
using Core.Bridge.Web;
#endif
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Bootstrap.States.Assets
{
    public class AddressableAssetBootstrapper : IAssetBootstrapper
    {
        private readonly AssetProvider _assetProvider = new();
        private readonly List<ScriptableObject> _settings = new();
        private readonly Dictionary<Type, IRuntimeSystem> _systems = new();

        private readonly string[] _systemAddresses = {
            ObjectPoolingSystem.Address,
            AudioSystem.Address,
            ParticlePoolSystem.Address,
            OverlayNotificationSystem.Address,
            UIFadeSystem.Address,
#if UNITY_WEBGL
            EventDispatcher.Address,
#endif
        };

        private readonly AssetReference[] _settingsAssets;

        public AddressableAssetBootstrapper(AssetReference[] settingsAssets)
        {
            _settingsAssets = settingsAssets;
        }

        async UniTask IAssetBootstrapper.Initialize()
        {
            await _assetProvider.Initialize();

            foreach (var address in _systemAddresses)
            {
                var go = await _assetProvider.Load<GameObject>(address);
                var system = go.GetComponent<IRuntimeSystem>();
                _systems[system.GetType()] = system;
            }

            foreach (var settingRef in _settingsAssets)
            {
                var setting = await _assetProvider.Load<ScriptableObject>(settingRef);
                _settings.Add(setting);
            }
        }

        IEnumerable<IRuntimeSystem> IAssetBootstrapper.GetLoadedSystems()
        {
            return _systems.Values;
        }

        IEnumerable<ScriptableObject> IAssetBootstrapper.GetLoadedSettings()
        {
            return _settings;
        }

        T IAssetBootstrapper.GetSystem<T>()
        {
            return (T)_systems[typeof(T)];
        }

        bool IAssetBootstrapper.TryGetSystem<T>(out T manager)
        {
            if (_systems.TryGetValue(typeof(T), out var baseManager))
            {
                manager = (T)baseManager;
                return true;
            }

            manager = default;
            return false;
        }
    }
}