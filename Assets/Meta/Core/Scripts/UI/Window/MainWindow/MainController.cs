using Core.Services;
using Core.UI.MVC;
using VContainer;

namespace Core.UI
{
    public class MainController : Controller<MainModel, MainWindow>
    {
        private LevelFlowController _levelFlowController;
        private IInputService _inputService;

        [Inject]
        private void Construct(LevelFlowController levelFlowController, IInputService inputService)
        {
            _levelFlowController = levelFlowController;
            _inputService = inputService;
        }

        public override void Bind()
        {
            base.Bind();
            
            _inputService.Clicked += IInputService_Clicked;
        }

        public override void Dispose()
        {
            base.Dispose();
            
            _inputService.Clicked -= IInputService_Clicked;
        }

        private void IInputService_Clicked()
        {
            if (!_levelFlowController.IsStarted)
            {
                _levelFlowController.Start();
            }
        }
    }
}