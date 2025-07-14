namespace Core.Services
{
    public interface IAnalyticsService
    {
        void TrackStart(int levelIndex);
        void TrackFinish();
        void TrackFail(string reason);
    }
}