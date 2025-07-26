using Core.Bootstrap.States.Assets;

namespace Core.Bootstrap.States
{
    public class RegisterState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ProjectContextInstaller _projectContextInstaller;

        public RegisterState(GameStateMachine stateMachine, ProjectContextInstaller projectContextInstaller)
        {
            _stateMachine = stateMachine;
            _projectContextInstaller = projectContextInstaller;
        }
        
        void IState.Enter()
        {
            _projectContextInstaller.RegisterAll();
            
            _stateMachine.Enter<ResolveState>();
        }

        public void Exit() { }
    }
}