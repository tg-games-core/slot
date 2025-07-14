using R3;

namespace Core.Data
{
    public interface IProgressData
    {
        ReadOnlyReactiveProperty<int> LevelIndex
        {
            get;
        }
        
        void SetLevelIndex(int levelIndex);
    }
}