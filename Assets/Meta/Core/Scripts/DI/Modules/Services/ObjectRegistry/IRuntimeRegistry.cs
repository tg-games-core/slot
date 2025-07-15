using Project.Plinko.Interfaces;
using Project.Score.Interfaces;

namespace Core.Services
{
    public interface IRuntimeRegistry
    {
        IPlinkoService PlinkoService { get; }
        IScoreService ScoreService { get; }
        
        void RegisterPlinkoService(IPlinkoService plinkoService);
        void RegisterScoreService(IScoreService scoreService);
    }
}