using VContainer;
using VContainer.Unity;

namespace Core
{
    public class GameplayInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            
            builder.Register<SceneBootstrapper>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}