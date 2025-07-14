using Core.Bootstrap.States.Assets;

namespace Core.Bootstrap.States
{
    public class WarmUpState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IAssetBootstrapper _assetBootstrapper;

        public WarmUpState(GameStateMachine stateMachine, IAssetBootstrapper assetBootstrapper)
        {
            _stateMachine = stateMachine;
            _assetBootstrapper = assetBootstrapper;
        }
        
        async void IState.Enter()
        {
            await _assetBootstrapper.Initialize();
            
            _stateMachine.Enter<RegisterState>();
        }
        
        public void Exit() { }
    }
}