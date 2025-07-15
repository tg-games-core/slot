using Project.Plinko.Types;
using R3;

namespace Project.Plinko.Interfaces
{
    public interface IPlinkoService
    {
        ReadOnlyReactiveProperty<float> Balance { get; }
        ReadOnlyReactiveProperty<PlinkoStateType> StateType { get; }
        
        void StartGame();
        void EndGame();
        void FailGame();
    }
}