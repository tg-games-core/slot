using Project.Plinko.Interfaces;
using Project.Score.Interfaces;

namespace Core.Services
{
    public class RuntimeRegistry : IRuntimeRegistry
    {
        private IPlinkoService _plinkoService;
        private IScoreService _scoreService;

        IPlinkoService IRuntimeRegistry.PlinkoService
        {
            get => _plinkoService;
        }

        IScoreService IRuntimeRegistry.ScoreService
        {
            get => _scoreService;
        }

        void IRuntimeRegistry.RegisterPlinkoService(IPlinkoService plinkoService)
        {
            _plinkoService = plinkoService;
        }

        void IRuntimeRegistry.RegisterScoreService(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }
    }
}