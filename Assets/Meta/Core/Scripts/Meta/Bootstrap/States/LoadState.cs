using Core.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Core.Bootstrap.States
{
    public class LoadState : IState
    {
        private const string MainUI = "UICommon";

        private readonly GameStateMachine _stateMachine;
        private readonly SlicedFilledImage _loadingProgress;
        private readonly LoadingSettings _loadingSettings;

        private LevelFlowController _levelFlowController;
        private SceneLoadingService _sceneLoadingService;

        public LoadState(GameStateMachine stateMachine, LoadingSettings loadingSettings, SlicedFilledImage loadingProgress)
        {
            _stateMachine = stateMachine;
            _loadingSettings = loadingSettings;
            _loadingProgress = loadingProgress;
        }

        [Inject]
        private void Construct(LevelFlowController levelFlowController, SceneLoadingService sceneLoadingService)
        {
            _sceneLoadingService = sceneLoadingService;
            _levelFlowController = levelFlowController;
        }
        
        async void IState.Enter()
        {
            float time = 0f;

            void updateLoadingProgress()
            {
                _loadingProgress.fillAmount = Mathf.Clamp(time / _loadingSettings.LoadingTime, 0, 1);
            }

            await _sceneLoadingService.Load(MainUI, (progress) =>
            {
                updateLoadingProgress();
                time += Time.deltaTime;
            });
            
            var levelWaiter = _levelFlowController.Load();
            while (levelWaiter.Status == UniTaskStatus.Pending || time < _loadingSettings.LoadingTime)
            {
                updateLoadingProgress();
                
                await UniTask.Yield();
            
                time += Time.deltaTime;
            }
            
            _loadingProgress.fillAmount = 1f;
            
            _stateMachine.Enter<GameRunnerState>();
        }

        public void Exit() { }
    }
}