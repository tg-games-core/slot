using System;
using System.Linq;
using ObservableCollections;
using VContainer;
using VContainer.Unity;

namespace Project.Bounce.Containers
{
    public class DiceContainer : IInitializable, IDisposable
    {
        private readonly ObservableList<PlinkoDice> _dices = new();
        
        private GapController _gapController;

        public IReadOnlyObservableList<PlinkoDice> Dices
        {
            get => _dices;
        }

        [Inject]
        private void Construct(GapController gapController)
        {
            _gapController = gapController;
        }

        void IInitializable.Initialize()
        {
            _gapController.DiceFell += GapController_DiceFell;
        }

        void IDisposable.Dispose()
        {
            _gapController.DiceFell -= GapController_DiceFell;
        }
        
        public void RegisterBouncingDice(PlinkoDice dice)
        {
            _dices.Add(dice);
        }

        public void Clear()
        {
            var dices = _dices.ToArray();
            for (int i = 0; i < dices.Length; i++)
            {
                FreeDice(dices[i]);
            }
            
            _dices.Clear();
        }

        private void FreeDice(PlinkoDice dice)
        {
            dice.Free();
            _dices.Remove(dice);
        }
        
        private void GapController_DiceFell(PlinkoDice dice)
        {
            FreeDice(dice);
        }
    }
}