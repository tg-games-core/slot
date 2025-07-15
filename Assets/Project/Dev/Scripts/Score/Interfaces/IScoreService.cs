using R3;

namespace Project.Score.Interfaces
{
    public interface IScoreService
    {
        ReadOnlyReactiveProperty<int> HitCount { get; }
        ReadOnlyReactiveProperty<int> LostDiceCount { get; }
        ReadOnlyReactiveProperty<float> Multiplier { get; }
    }
}