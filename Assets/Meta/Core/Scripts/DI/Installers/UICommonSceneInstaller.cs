using System.Linq;
using Core.UI;
using Core.Services;
using Core.UI.MVC.Interface;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core
{
    public class UICommonSceneInstaller : LifetimeDontDestroyInstaller
    {
        [SerializeField]
        private UISystem _uiSystem;
        
        [SerializeField]
        private JoystickController _joystickController;

        [Inject]
        private void Construct(LevelFlowController levelFlowController)
        {
            levelFlowController.InjectUISystem(_uiSystem);
        }

        protected override void Awake()
        {
            base.Awake();

            RegisterUI();
            Container.InjectGameObject(_joystickController.gameObject);
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_uiSystem);
            RegisterInputService(builder);
        }

        private void RegisterUI()
        {
            var components = _uiSystem.transform.GetComponentsInChildren<IView>(true)
                .Select(v => v.Transform.gameObject).ToArray();
            
            Container.InjectInHierarchy(components);
        }

        private void RegisterInputService(IContainerBuilder builder)
        {
            IInputService inputService = new MobileInputService(_joystickController);
            builder.RegisterInstance(inputService);
            inputService?.Init();
        }
    }    
}