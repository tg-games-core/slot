using System;

namespace Core.Bootstrap.States
{
    public class ResolveState : IState
    {
        private readonly Type[] _forceInjectTypes = { typeof(LoadState) };

        private readonly GameStateMachine _stateMachine;
        private readonly ProjectContextInstaller _projectContextInstaller;

        public ResolveState(GameStateMachine stateMachine, ProjectContextInstaller projectContextInstaller)
        {
            _stateMachine = stateMachine;
            _projectContextInstaller = projectContextInstaller;
        }
        
        void IState.Enter()
        {
            _projectContextInstaller.ResolveAll();

            for (int i = 0; i < _forceInjectTypes.Length; i++)
            {
                _projectContextInstaller.Container.Inject(_stateMachine.GetState(_forceInjectTypes[i]));
            }
            
            _stateMachine.Enter<LoadState>();
        }

        public void Exit() { }
    }
}