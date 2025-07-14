using Core.Bootstrap.States.Assets;

namespace Core.Bootstrap.States
{
    public class RegisterState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ProjectContextInstaller _projectContextInstaller;
        private readonly IAssetBootstrapper _assetBootstrapper;

        public RegisterState(GameStateMachine stateMachine, ProjectContextInstaller projectContextInstaller, 
            IAssetBootstrapper assetBootstrapper)
        {
            _stateMachine = stateMachine;
            _projectContextInstaller = projectContextInstaller;
            _assetBootstrapper = assetBootstrapper;
        }
        
        void IState.Enter()
        {
            _projectContextInstaller.RegisterAll(_assetBootstrapper);
            
            _stateMachine.Enter<ResolveState>();
        }

        public void Exit() { }
    }
}