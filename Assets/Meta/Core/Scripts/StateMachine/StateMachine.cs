namespace Core.StateMachine
{
    public abstract class StateMachine
    {
        protected IState _currentState;
        
        protected StateMachine(IState initialState)
        {
            SetCurrentState(initialState);
        }
        
        private void SetCurrentState(IState state)
        {
            _currentState = state;
            
            state.OnStateEnter();
        }
        
        public void TryChangeState()
        {
            var transitionIndex = IsTransitionsCondition();
            
            if (transitionIndex != -1)
            {
                var activeTransition = _currentState.Transitions[transitionIndex];
                
                _currentState.OnStateExit();
                SetCurrentState(activeTransition.To);
            }
        }
        
        private int IsTransitionsCondition()
        {
            var currentTransitions = _currentState.Transitions;

            for (var i = 0; i < currentTransitions.Count; i++)
            {
                var condition = currentTransitions[i].Condition;

                if (condition.Invoke())
                {
                    return i;
                }
            }

            return -1;
        }
    }
}