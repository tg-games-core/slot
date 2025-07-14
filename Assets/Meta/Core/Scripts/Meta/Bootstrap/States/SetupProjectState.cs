using UnityEngine;

namespace Core.Bootstrap.States
{
    public class SetupProjectState : IState
    {
        private readonly GameStateMachine _stateMachine;

        public SetupProjectState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        void IState.Enter()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            Screen.orientation = ScreenOrientation.Portrait;
            
            _stateMachine.Enter<WarmUpState>();
        }

        public void Exit() { }
    }
}