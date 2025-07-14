using Core.Bootstrap.States;
using Core.Bootstrap.States.Assets;
using Core.UI;
using UnityEngine;

namespace Core.Bootstrap
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField]
        private ProjectContextInstaller _projectContextInstaller;
        
        [SerializeField, Header("Loading")]
        private SlicedFilledImage _loadingProgress;

        [SerializeField]
        private CanvasGroup[] _canvasGroups;

        [SerializeField, Header("Settings")]
        private LoadingSettings _loadingSettings;
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            var assetBootstrapper = new AddressableAssetBootstrapper(_projectContextInstaller.SettingsAssets);
            var stateMachine = new GameStateMachine(_projectContextInstaller, this, assetBootstrapper, _loadingProgress,
                _canvasGroups, _loadingSettings);
            
            stateMachine.Enter<SetupProjectState>();
        }
    }
}