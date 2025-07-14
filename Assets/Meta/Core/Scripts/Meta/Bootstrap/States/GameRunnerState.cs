using System.Threading;
using UnityEngine;

namespace Core.Bootstrap.States
{
    public class GameRunnerState : IState
    {
        private readonly LoadingSettings _loadingSettings;
        private readonly CanvasGroup[] _canvasGroups;
        private readonly Bootstrapper _bootstrapper;

        public GameRunnerState(LoadingSettings loadingSettings, CanvasGroup[] canvasGroups, Bootstrapper bootstrapper)
        {
            _loadingSettings = loadingSettings;
            _canvasGroups = canvasGroups;
            _bootstrapper = bootstrapper;
        }
        
        async void IState.Enter()
        {
            foreach (var canvasGroup in _canvasGroups)
            {
                await UniTaskExtensions.Lerp(progress =>
                    {
                        canvasGroup.alpha = 1 - progress;
                    }, 
                    _loadingSettings.FadeTime, _loadingSettings.FadeCurve, CancellationToken.None); 
            }
            
            Object.Destroy(_bootstrapper.gameObject);
        }

        public void Exit() { }
    }
}