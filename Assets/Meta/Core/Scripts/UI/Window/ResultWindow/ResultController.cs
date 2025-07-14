using Core.UI.MVC;
using VContainer;

namespace Core.UI
{
    public class ResultController : Controller<ResultModel, ResultWindow>
    {
        private LevelFlowController _levelFlowController;

        [Inject]
        private void Construct(LevelFlowController levelFlowController)
        {
            _levelFlowController = levelFlowController;
        }
        
        public override void Bind()
        {
            base.Bind();
            
            _view.OnContinueClicked += View_OnContinueClicked;
        }

        public override void Dispose()
        {
            base.Dispose();
            
            _view.OnContinueClicked -= View_OnContinueClicked;
        }

        protected override void UpdateView() { }

        private void View_OnContinueClicked()
        {
            _levelFlowController?.Load();
        }
    }
}