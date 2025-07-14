namespace Core.Services
{
    public class DefaultAnalyticsService : IAnalyticsService
    {
        void IAnalyticsService.TrackStart(int levelIndex)
        {
            DebugSafe.Log($"Track Start - {levelIndex}");
        }

        void IAnalyticsService.TrackFinish()
        {
            DebugSafe.Log($"Track Finish");
        }

        void IAnalyticsService.TrackFail(string reason)
        {
            DebugSafe.Log($"Track Fail: {reason}");
        }
    }
}