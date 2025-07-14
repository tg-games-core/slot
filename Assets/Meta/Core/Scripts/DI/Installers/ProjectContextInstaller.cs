using System;
using System.Collections.Generic;
using Core.Bootstrap.States.Assets;
#if UNITY_WEBGL
using Core.Bridge.Web;
#endif
using Core.Data;
using Core.Services;
using Core.Services.Implementations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;
#if FORCE_DEBUG
using Core.Debug;
using VContainer.Unity;
#endif

namespace Core
{
    public class ProjectContextInstaller : LifetimeDontDestroyInstaller
    {
        private readonly List<Type> _services = new();

        [field: SerializeField, Header("Settings")]
        public AssetReference[] SettingsAssets
        {
            get; private set;
        }

        private IAssetBootstrapper _assetBootstrapper;
        
        public void RegisterAll(IAssetBootstrapper assetBootstrapper)
        {
            _assetBootstrapper = assetBootstrapper;
            
            Build();
        }

        public void ResolveAll()
        {
            for (int i = 0; i < _services.Count; i++)
            {
                if (Container.Resolve(_services[i]) is IService service)
                {
                    service.Init();
                }
            }
        }

        protected override void Configure(IContainerBuilder builder)
        {
            BindSettings(builder);
            BindControllers(builder);
            BindManagers(builder);
            BindServices(builder);
            BindMeta(builder);
        }

        private void BindSettings(IContainerBuilder builder)
        {
            foreach (var settings in _assetBootstrapper.GetLoadedSettings())
            {
                builder.RegisterInstance(settings).AsSelf();
            }
        }

        private void BindControllers(IContainerBuilder builder)
        {
            builder.Register<SceneLoadingService>(Lifetime.Singleton);
            builder.Register<LevelFlowController>(Lifetime.Singleton);
            builder.Register<ShadowOptimizationSystem>(Lifetime.Singleton);
            builder.Register<CameraService>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void BindManagers(IContainerBuilder builder)
        {
            RegisterManager<ObjectPoolingSystem, IPoolSystem>(builder);
            RegisterManager<AudioSystem, IAudioSystem>(builder);
            RegisterManager<ParticlePoolSystem, IParticleSystem>(builder);
            RegisterManager<OverlayNotificationSystem>(builder);
            RegisterManager<UIFadeSystem>(builder);
            
#if UNITY_WEBGL
            RegisterManager<EventDispatcher>(builder);
#endif

#if FORCE_DEBUG
            builder.Register<CustomDebugMenu>(Lifetime.Singleton);
            builder.RegisterEntryPoint<DebugMenu>();
#endif
        }

        private void BindServices(IContainerBuilder builder)
        {
            _services.Add(RemoteConfigServiceController.RegisterService(builder));
            _services.Add(AdvertisingServiceController.RegisterService(builder));
            _services.Add(AnalyticsServiceController.RegisterService(builder));
            _services.Add(InAppServicePurchaseController.RegisterService(builder));
            _services.Add(RateUsServiceController.RegisterService(builder));
            _services.Add(HapticServiceController.RegisterService(builder));
        }

        private void BindMeta(IContainerBuilder builder)
        {
            builder.Register<Storage>(Lifetime.Singleton);
            builder.Register<ProgressData>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LevelData>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<Consumable>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<User>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<RuntimeRegistry>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterManager<TManager>(IContainerBuilder builder)
            where TManager : RuntimeComponent<TManager>
        {
            var manager = _assetBootstrapper.GetSystem<TManager>();
            builder.RegisterComponentInNewPrefab(manager, Lifetime.Singleton).DontDestroyOnLoad().AsSelf();
        }

        private void RegisterManager<TManager, TSystem>(IContainerBuilder builder)
            where TManager : RuntimeComponent<TManager>
        {
            var manager = _assetBootstrapper.GetSystem<TManager>();
            builder.RegisterComponentInNewPrefab(manager, Lifetime.Singleton).DontDestroyOnLoad().AsSelf()
                .As<TSystem>();
        }
    }
}