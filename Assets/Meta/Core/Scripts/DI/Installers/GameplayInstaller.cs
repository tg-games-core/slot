using Project.Autoplay;
using Project.Bounce.Containers;
using Project.Plinko;
using Project.Score;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class GameplayInstaller : LifetimeScope
    {
        [SerializeField]
        private GapController _gapController;
        
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterComponent(_gapController).AsSelf();
            
            builder.Register<DiceContainer>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.Register<PlinkoService>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<ScoreService>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<SceneBootstrapper>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<AutoplayService>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}