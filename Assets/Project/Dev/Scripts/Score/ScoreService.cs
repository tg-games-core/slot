using System;
using Core;
using Core.Services;
using ObservableCollections;
using Project.Bounce;
using Project.Bounce.Containers;
using Project.Bounce.Settings;
using Project.Plinko.Interfaces;
using Project.Plinko.Settings;
using Project.Plinko.Types;
using Project.Score.Interfaces;
using R3;
using VContainer;
using VContainer.Unity;

namespace Project.Score
{
    public class ScoreService : IScoreService, IInitializable, IDisposable
    {
        private readonly ReactiveSubscribersContainer _reactiveContainer = new();
        private readonly ReactiveProperty<int> _lostDiceCount = new();
        private readonly ReactiveProperty<int> _hitCount = new();
        private readonly ReactiveProperty<float> _multiplier = new();

        private IRuntimeRegistry _runtimeRegistry;
        private IPlinkoService _plinkoService;
        private DiceContainer _diceContainer;
        private PlinkoSettings _plinkoSettings;
        private DiceSpawnSettings _diceSpawnSettings;
        private GapController _gapController;

        public ReadOnlyReactiveProperty<int> HitCount
        {
            get => _hitCount;
        }

        public ReadOnlyReactiveProperty<int> LostDiceCount
        {
            get => _lostDiceCount;
        }

        public ReadOnlyReactiveProperty<float> Multiplier
        {
            get => _multiplier;
        }

        [Inject]
        private void Construct(IRuntimeRegistry runtimeRegistry, IPlinkoService plinkoService,
            DiceContainer diceContainer, GapController gapController, PlinkoSettings plinkoSettings, 
            DiceSpawnSettings diceSpawnSettings)
        {
            _gapController = gapController;
            _runtimeRegistry = runtimeRegistry;
            _plinkoService = plinkoService;
            _diceContainer = diceContainer;
            _plinkoSettings = plinkoSettings;
            _diceSpawnSettings = diceSpawnSettings;
        }
        
        void IInitializable.Initialize()
        {
            _runtimeRegistry.RegisterScoreService(this);
            
            _reactiveContainer.Subscribe(_plinkoService.StateType, OnPlinkoStateChanged);
            
            _reactiveContainer.Subscribe(_diceContainer.Dices.ObserveAdd(), OnDiceAdded);
            _reactiveContainer.Subscribe(_diceContainer.Dices.ObserveRemove(), OnDiceRemoved);
            
            _gapController.DiceFell += GapController_DiceFell;
        }
        
        void IDisposable.Dispose()
        {
            _reactiveContainer?.Dispose();
            
            _gapController.DiceFell -= GapController_DiceFell;
        }

        private void ResetScore()
        {
            _lostDiceCount.Value = 0;
            _hitCount.Value = 0;
            _multiplier.Value = 1f;
        }
        
        private void OnPlinkoStateChanged(PlinkoStateType stateType)
        {
            switch (stateType)
            {
                case PlinkoStateType.Idle:
                case PlinkoStateType.WaitingForCashout:
                    ResetScore();
                    break;
            }
        }

        private void OnDiceAdded(CollectionAddEvent<PlinkoDice> addedDice)
        {
            addedDice.Value.Bounced += PlinkoDice_Bounced;
        }

        private void OnDiceRemoved(CollectionRemoveEvent<PlinkoDice> removedDice)
        {
            removedDice.Value.Bounced -= PlinkoDice_Bounced;
        }        
        
        private void GapController_DiceFell(PlinkoDice dice)
        {
            _lostDiceCount.Value++;
            
            if (_lostDiceCount.Value == 1)
            {
                _multiplier.Value *= _plinkoSettings.MultiplierPenaltyOnDiceLoss;
            }

            if (_lostDiceCount.Value >= _diceSpawnSettings.DiceCount)
            {
                _plinkoService.FailGame();
            }
        }

        private void PlinkoDice_Bounced()
        {
            _multiplier.Value += _plinkoSettings.MultiplierPerHit;
            _hitCount.Value++;
        }
    }
}