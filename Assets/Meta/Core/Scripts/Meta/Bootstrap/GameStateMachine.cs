using System;
using System.Collections.Generic;
using Core.Bootstrap.Interface;
using Core.Bootstrap.States;
using Core.Bootstrap.States.Assets;
using Core.UI;
using UnityEngine;

namespace Core.Bootstrap
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        
        private IExitableState _activeState;

        public GameStateMachine(ProjectContextInstaller installer, Bootstrapper bootstrapper, SlicedFilledImage loadingProgress, CanvasGroup[] canvasGroups,
            LoadingSettings loadingSettings)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(SetupProjectState)] = new SetupProjectState(this),
                [typeof(WarmUpState)] = new WarmUpState(this),
                [typeof(RegisterState)] = new RegisterState(this, installer),
                [typeof(ResolveState)] = new ResolveState(this, installer),
                [typeof(LoadState)] = new LoadState(this, loadingSettings, loadingProgress),
                [typeof(GameRunnerState)] = new GameRunnerState(loadingSettings, canvasGroups, bootstrapper),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        public TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
        
        public IExitableState GetState(Type type)
        {
            return _states[type];
        }
    }
}