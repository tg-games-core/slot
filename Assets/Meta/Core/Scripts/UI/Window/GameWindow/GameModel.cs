using Core.UI.MVC;
using Project.Plinko.Settings;
using VContainer;

namespace Core.UI
{
    public class GameModel : Model
    {
        private PlinkoSettings _plinkoSettings;

        public float GameCost
        {
            get => _plinkoSettings.GameCost;
        }
        
        [Inject]
        private void Construct(PlinkoSettings plinkoSettings)
        {
            _plinkoSettings = plinkoSettings;
        }
    }
}