using VContainer.Unity;

namespace Core
{
    public abstract class LifetimeDontDestroyInstaller : LifetimeScope
    {
        protected override void Awake()
        {
            base.Awake();
            
            DontDestroyOnLoad(gameObject);
        }
    }
}