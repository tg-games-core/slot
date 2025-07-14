#if UNITY_WEBGL
using System;

namespace Core.Bridge.Web
{
    [Serializable]
    public struct EventHeader
    {
        public string EventName;

        public EventHeader(string eventName)
        {
            EventName = eventName;
        }
    }
}
#endif