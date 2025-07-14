#if UNITY_WEBGL
using System;

namespace Core.Bridge.Web
{
    [Serializable]
    public sealed class Event<T> where T : EventPayload
    {
        public string EventName;
        public T Payload;

        public Event(string eventName, T payload)
        {
            EventName = eventName;
            Payload = payload;
        }
    }
}
#endif