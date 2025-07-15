using Core.Services;
using Project.Plinko.Interfaces;
using Project.Plinko.Settings;
using Project.Plinko.Types;
using R3;
using VContainer;
using VContainer.Unity;

namespace Project.Plinko
{
    public class PlinkoService : IPlinkoService, IInitializable
    {
        private readonly ReactiveProperty<float> _balance = new();
        private readonly ReactiveProperty<PlinkoStateType> _stateType = new();
        
        private IRuntimeRegistry _runtimeRegistry;
        private PlinkoSettings _plinkoSettings;

        public ReadOnlyReactiveProperty<float> Balance
        {
            get => _balance;
        }

        ReadOnlyReactiveProperty<PlinkoStateType> IPlinkoService.StateType
        {
            get => _stateType;
        }

        [Inject]
        private void Construct(IRuntimeRegistry runtimeRegistry, PlinkoSettings plinkoSettings)
        {
            _runtimeRegistry = runtimeRegistry;
            _plinkoSettings = plinkoSettings;
        }
        
        void IInitializable.Initialize()
        {
            _runtimeRegistry.RegisterPlinkoService(this);

            _balance.Value = _plinkoSettings.StartBalance;
        }

        void IPlinkoService.StartGame()
        {
            if (_stateType.Value != PlinkoStateType.WaitingForCashout)
            {
                _stateType.Value = PlinkoStateType.WaitingForCashout;

                _balance.Value -= _plinkoSettings.GameCost;
            }
        }

        void IPlinkoService.EndGame()
        {
            _stateType.Value = PlinkoStateType.Cashout;
            
            var reward = _plinkoSettings.GameCost * _runtimeRegistry.ScoreService.Multiplier.CurrentValue;
            _balance.Value += reward;
        }

        void IPlinkoService.FailGame()
        {
            _stateType.Value = PlinkoStateType.Cashout;
        }
    }
}