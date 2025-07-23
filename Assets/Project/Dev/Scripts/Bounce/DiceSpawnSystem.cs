using System;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Project.Bounce.Containers;
using Project.Bounce.Settings;
using Project.Plinko.Interfaces;
using Project.Plinko.Types;
using UnityEngine;
using VContainer;

namespace Project.Bounce
{
    public class DiceSpawnSystem : MonoBehaviour
    {
        private readonly UniTaskAsyncContainer _asyncContainer = new();
        private readonly ReactiveSubscribersContainer _reactiveContainer = new();

        [SerializeField]
        private Transform _spawnPoint;
        
        private IPoolSystem _poolSystem;
        private IPlinkoService _plinkoService;
        private DiceContainer _diceContainer;
        private DiceSpawnSettings _diceSpawnSettings;

        private CancellationTokenSource _spawnToken;

        [Inject]
        private void Construct(IPoolSystem poolSystem, IPlinkoService plinkoService, 
            DiceContainer diceContainer, DiceSpawnSettings diceSpawnSettings)
        {
            _poolSystem = poolSystem;
            _plinkoService = plinkoService;
            _diceContainer = diceContainer;
            _diceSpawnSettings = diceSpawnSettings;
        }

        private void OnEnable()
        {
            _reactiveContainer.Subscribe(_plinkoService.StateType, OnPlinkoStateChanged);
        }

        private void OnDisable()
        {
            _reactiveContainer.FreeSubscribers();   
        }

        private void SpawnBouncingItems()
        {
            SpawnBouncingItemsAsync(_asyncContainer.RefreshToken(ref _spawnToken)).Forget();
        }

        private void FreeBouncingItems()
        {
            _diceContainer.Clear();
            
            _asyncContainer.CancelToken(ref _spawnToken);
        }

        private async UniTaskVoid SpawnBouncingItemsAsync(CancellationToken token)
        {
            for (int i = 0; i < _diceSpawnSettings.DiceCount; i++)
            {
                var dice = _poolSystem.Get<PlinkoDice>(_diceSpawnSettings.GetDice(i), _spawnPoint.position,
                    Quaternion.identity);
            
                _diceContainer.RegisterBouncingDice(dice);

                await UniTask.Delay(TimeSpan.FromSeconds(_diceSpawnSettings.SpawnInterval),
                    cancellationToken: token);
            }

            _asyncContainer.CancelToken(ref _spawnToken);
        }
        
        private void OnPlinkoStateChanged(PlinkoStateType stateType)
        {
            switch (stateType)
            {
                case PlinkoStateType.WaitingForCashout:
                    SpawnBouncingItems();
                    break;
                
                default:
                    FreeBouncingItems();
                    break;
            }
        }
    }
}