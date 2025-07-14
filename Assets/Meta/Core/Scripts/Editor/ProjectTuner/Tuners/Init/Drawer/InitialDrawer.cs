using UnityEditor;
using UnityEditor.Build;

namespace Core.Editor.Tuner.Drawer
{
    public abstract class InitialDrawer
    {
        public abstract ManagedStrippingLevel SelectedStrippingLevel
        {
            get;
        }
        
        protected ManagedStrippingLevel CurrentStrippingLevel
        {
            get => PlayerSettings.GetManagedStrippingLevel(NamedBuildTarget);
        }

        protected abstract NamedBuildTarget NamedBuildTarget
        {
            get;
        }

        protected InitialDrawer()
        {
            Initialize();
        }

        protected abstract void Initialize(); 
        public abstract void Draw();
    }
}