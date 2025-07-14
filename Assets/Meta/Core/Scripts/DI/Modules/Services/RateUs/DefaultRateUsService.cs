namespace Core.Services
{
    public class DefaultRateUsService : IRateUsService
    {
        void IRateUsService.ShowRateUs()
        {
            DebugSafe.Log("ShowRateUs");
        }
    }
}