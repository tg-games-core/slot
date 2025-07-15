using System;
using System.Collections.Generic;
using Core.Data;
using Core.Services;
using Core.UI;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Core
{
    public class LevelFlowController
    {
        private readonly IProgressData _progressData;
        private readonly ILevelData _levelData;
        private readonly IAnalyticsService _analyticsService;
        private readonly LevelSettings _levelSettings;
        private readonly SceneLoadingService _sceneLoadingService;
        
        public event Action Started;
        public event Action<bool> Finished;

        private UISystem _uiSystem;

        public bool IsStarted
        {
            get;
            private set;
        }

        [Inject]
        public LevelFlowController(LevelSettings levelSettings, IProgressData progressData, ILevelData levelData,
            IAnalyticsService analyticsService, SceneLoadingService sceneLoadingService)
        {
            _sceneLoadingService = sceneLoadingService;
            _levelSettings = levelSettings;
            _progressData = progressData;
            _levelData = levelData;
            _analyticsService = analyticsService;
        }

        public void Start(Action callback = null)
        {
            if (!IsStarted)
            {
                IsStarted = true;
                
                _analyticsService.TrackStart(_progressData.LevelIndex.CurrentValue);

                Started?.Invoke();

                _uiSystem.ShowWindow<GameWindow>();
                
                callback?.Invoke();
            }
            else
            {
                DebugSafe.LogError($"Trying to start level twice! This method should be called once at level start");
            }
        }
        
        public async void Complete(Dictionary<string, object> data = null, Action callback = null)
        {
            if (IsStarted)
            {
                IsStarted = false;
                
                _analyticsService.TrackFinish();

                OnFinish(true, callback);

                _progressData.SetLevelIndex(_progressData.LevelIndex.CurrentValue + 1);

                await UniTask.Delay(TimeSpan.FromSeconds(_levelSettings.ResultDelay));

                _uiSystem.ShowWindow<ResultWindow>(data);
            }
            else
            {
                DebugSafe.LogError(
                    $"Trying to finish level twice! This method should be called once after method: {nameof(Start)}");
            }
        }

        public async void Fail(Action callback = null)
        {
            if (IsStarted)
            {
                IsStarted = false;
                
                _analyticsService.TrackFail(string.Empty);

                OnFinish(false, callback);

                await UniTask.Delay(TimeSpan.FromSeconds(_levelSettings.FailDelay));

                _uiSystem.ShowWindow<FailWindow>();
            }
            else
            {
                DebugSafe.LogError(
                    $"Trying to finish level twice! This method should be called once after method: {nameof(Start)}");
            }
        }

        private void OnFinish(bool isWin, Action callback)
        {
            Finished?.Invoke(isWin);
            
            _levelData.Reset();
            
            callback?.Invoke();
        }
        
        public async UniTask Load(Action callback = null)
        {
            await _sceneLoadingService.Load(_levelSettings.GetScene(_progressData.LevelIndex.CurrentValue));
            
            callback?.Invoke();
            
            _uiSystem.ShowWindow<GameWindow>();
        }

        public void InjectUISystem(UISystem uiSystem)
        {
            _uiSystem = uiSystem;
        }
    }
}