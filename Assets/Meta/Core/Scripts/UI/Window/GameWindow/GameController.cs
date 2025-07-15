using Core.Services;
using Core.UI.MVC;
using Project.Plinko.Types;
using UnityEngine;
using VContainer;

namespace Core.UI
{
    public class GameController : Controller<GameModel, GameWindow>
    {
        private float _balance;
        
        private IRuntimeRegistry _runtimeRegistry;

        [Inject]
        private void Construct(IRuntimeRegistry runtimeRegistry)
        {
            _runtimeRegistry = runtimeRegistry;
        }

        public override void Bind()
        {
            base.Bind();
            
            _reactiveContainer.Subscribe(_runtimeRegistry.PlinkoService.Balance, OnBalanceChanged);
            _reactiveContainer.Subscribe(_runtimeRegistry.PlinkoService.StateType, OnPlinkoStateChanged);
            
            _reactiveContainer.Subscribe(_runtimeRegistry.ScoreService.Multiplier, OnMultiplierChanged);
            _reactiveContainer.Subscribe(_runtimeRegistry.ScoreService.HitCount, OnHitCountChanged);
        }

        protected override void OnShowing()
        {
            base.OnShowing();
            
            RefreshButtons();
            
            _view.HideTransaction();
        }

        public void Start()
        {
            _runtimeRegistry.PlinkoService.StartGame();
        }

        public void Cashout()
        {
            _runtimeRegistry.PlinkoService.EndGame();
        }
        
        private void OnBalanceChanged(float balance)
        {
            var delta = balance - _balance;
            _balance = balance;
            
            _view.RefreshBalance(balance);
            _view.ShowTransaction(delta, delta > 0 ? Color.green : Color.red);
        }
        
        private void OnPlinkoStateChanged(PlinkoStateType stateType)
        {
            RefreshButtons();

            switch (stateType)
            {
                case PlinkoStateType.Idle:
                    var message = _balance > _model.GameCost ?
                        "Нажмите 'Начать игру'" :
                        "Недостаточно средств для игры";
                    _view.SetupGameStatus(message);
                    break;
                
                case PlinkoStateType.Cashout:
                    _view.SetupGameStatus("Игра окончена");
                    break;
            }
        }
        
        private void OnMultiplierChanged(float multiplier)
        {
            _view.RefreshMultiplier(multiplier);
            _view.RefreshPotentialWin(_model.GameCost * multiplier);
            
            RefreshButtons();
        }
        
        private void OnHitCountChanged(int hitCount)
        {
            if (_runtimeRegistry.PlinkoService.StateType.CurrentValue == PlinkoStateType.WaitingForCashout)
            {
                //_view.SetupGameStatus($"Игра активна | Кубиков: {activeDice.Count}/{maxDiceCount}{lostStatus} | Попаданий: {hitCount}");
                _view.SetupGameStatus($"Игра активна | Попаданий: {hitCount}");
            }
        }

        private void RefreshButtons()
        {
            _view.RefreshButtons(_runtimeRegistry.PlinkoService.StateType.CurrentValue,
                _runtimeRegistry.ScoreService.Multiplier.CurrentValue);
        }
    }
}