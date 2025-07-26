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
//using UnityEngine.AddressableAssets;
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
        public ScriptableObject[] Settings
        {
            get; private set;
        }

        public void RegisterAll()
        {
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
            foreach (var settings in Settings)
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
            RegisterManager<ObjectPoolingSystem, IPoolSystem>(builder, ObjectPoolingSystem.GetManager);
            RegisterManager<AudioSystem, IAudioSystem>(builder, AudioSystem.GetManager);
            RegisterManager<ParticlePoolSystem, IParticleSystem>(builder, ParticlePoolSystem.GetManager);
            RegisterManager<OverlayNotificationSystem>(builder, OverlayNotificationSystem.GetManager);
            RegisterManager<UIFadeSystem>(builder, UIFadeSystem.GetManager);
            
#if UNITY_WEBGL
            RegisterManager<EventDispatcher>(builder, EventDispatcher.GetManager);
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

        private void RegisterManager<TManager>(IContainerBuilder builder, TManager managerPrefab)
            where TManager : RuntimeComponent<TManager>
        {
            builder.RegisterComponentInNewPrefab(managerPrefab, Lifetime.Singleton).DontDestroyOnLoad().AsSelf();
        }
        
        private void RegisterManager<TManager, TSystem>(IContainerBuilder builder, TManager managerPrefab)
            where TManager : RuntimeComponent<TManager>
        {
            builder.RegisterComponentInNewPrefab(managerPrefab, Lifetime.Singleton).DontDestroyOnLoad().AsSelf()
                .As<TSystem>();
        }
    }
}