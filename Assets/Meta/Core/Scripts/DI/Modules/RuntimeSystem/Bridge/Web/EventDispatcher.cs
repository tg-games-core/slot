#if UNITY_WEBGL
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace Core.Bridge.Web
{
    public sealed class EventDispatcher : RuntimeComponent<EventDispatcher>
    {
        private readonly Dictionary<string, Action<string, EventPayload>> _eventBindings = new();
        private readonly Dictionary<string, Type> _eventTypesBindings = new();
        
        [DllImport("__Internal")]
        private static extern void SendEvent(string serializedEvent);

        [Preserve]
        public void ProcessEvent(string serializedEvent)
        {
            var eventHeader = JsonUtility.FromJson<EventHeader>(serializedEvent);
            
            if (_eventBindings.ContainsKey(eventHeader.EventName))
            {
                var callback = _eventBindings[eventHeader.EventName];
                var receivedEvent =
                    (EventPayload) JsonUtility.FromJson(serializedEvent, _eventTypesBindings[eventHeader.EventName]);
                callback.Invoke(eventHeader.EventName, receivedEvent);
            }
        }

        public void RegisterHandler<T>(string eventName, Action<string, EventPayload> handler)
            where T : EventPayload
        {
            if (_eventBindings.ContainsKey(eventName))
            {
                _eventBindings[eventName] = handler;
                _eventTypesBindings[eventName] = typeof(T);
            }
            else
            {
                _eventBindings.Add(eventName, handler);
                _eventTypesBindings.Add(eventName, typeof(T));
            }
        }

        public void UnregisterHandler(string eventName)
        {
            if (_eventBindings.ContainsKey(eventName))
            {
                _eventBindings.Remove(eventName);
                _eventTypesBindings.Remove(eventName);
            }
        }

        public void Dispatch<T>(Event<T> @event) where T : EventPayload
        {
            SendEvent(JsonUtility.ToJson(@event));
        }
    }
}
#endif