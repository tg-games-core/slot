using System;
using System.Collections.Generic;

namespace Core.StateMachine
{
    public interface IState
    {
        List<Transition> Transitions
        {
            get; set;
        }
        
        bool CanEnter();
        void OnStateEnter();
        void OnStateExit();
        void AddTransition(IState to, Func<bool> condition);
    }
}