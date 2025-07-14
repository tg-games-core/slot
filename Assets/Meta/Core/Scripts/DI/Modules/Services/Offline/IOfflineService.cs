using System;

namespace Core.Services
{
    public interface IOfflineService
    {
        DateTime LastSession { get; }
        
        void SaveLastSession();
    }
}