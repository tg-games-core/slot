using System;
using VContainer.Unity;

namespace Core.Services
{
    public class OfflineService : IOfflineService, IInitializable, IDisposable
    {
        private IOfflineService _offlineService;
        
        DateTime IOfflineService.LastSession
        {
            get => LocalConfig.LastSessionTime;
        }
        
        void IOfflineService.SaveLastSession()
        {
            LocalConfig.LastSessionTime = DateTime.Now;
        }

        void IInitializable.Initialize()
        {
            _offlineService = this;
        }

        void IDisposable.Dispose()
        {
            _offlineService.SaveLastSession();
        }
    }
}