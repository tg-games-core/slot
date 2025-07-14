namespace Core.UI
{
    public class MainWindow : Window<MainModel, MainController>
    {
        public override bool IsPopup
        {
            get => false;
        }
    }
}