namespace Core.UI
{
    public class GameWindow : Window<GameModel, GameController>
    {
        public override bool IsPopup
        {
            get => false;
        }
    }
}