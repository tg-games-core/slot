namespace Core.UI
{
    public class FailWindow : Window<FailModel, FailController>
    {
        public override bool IsPopup
        {
            get => false;
        }
    }
}