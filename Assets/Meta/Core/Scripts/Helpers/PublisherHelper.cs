using System;
using System.Collections.Generic;

namespace Core
{
    public static class PublisherHelper
    {
        private static readonly Dictionary<PublisherType, string> PublisherDefines =
            new Dictionary<PublisherType, string>()
            {
                { PublisherType.None, string.Empty },
                { PublisherType.FireTV, "FireTV" }
            };

        public static PublisherType GetTargetCompany()
        {
            var targetStore = PublisherType.None;

#if FireTV
            targetStore = PublisherType.FireTV;
#endif
            
            return targetStore;
        }
        
        public static string GetDefineByPublisher(PublisherType publisherType)
        {
            if (PublisherDefines.TryGetValue(publisherType, out string define))
            {
                return define;
            }
            else
            {
                DebugSafe.LogException(new Exception($"Not found define for {nameof(PublisherType)} - {publisherType}"));
            }

            return string.Empty;
        }

        public static string[] GetPublisherDefines()
        {
            List<string> defines = new List<string>();

            foreach (var publisherDefine in PublisherDefines)
            {
                defines.Add(publisherDefine.Value);
            }

            return defines.ToArray();
        }
    }
}