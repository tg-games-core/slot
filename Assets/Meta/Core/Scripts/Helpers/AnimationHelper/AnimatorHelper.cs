using System;
using UnityEngine;

namespace Core
{
    public abstract class AnimatorHelper<T> : MonoBehaviour, IAnimationStateReader where T : Enum
    {
        public event Action<T> StateEntered;
        public event Action<T> StateExited;

        protected T _animatorState;
        
        public void EnteredState(int stateHash)
        {
            _animatorState = StateFor(stateHash);
            StateEntered?.Invoke(_animatorState);
        }

        public void ExitedState(int stateHash)
        {
            StateExited?.Invoke(StateFor(stateHash));
        }

        protected abstract T StateFor(int stateHash);
    }
}