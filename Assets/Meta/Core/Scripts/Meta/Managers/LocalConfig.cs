using System;
using UnityEngine;

namespace Core
{
    public static class LocalConfig
    {
        private class Keys
        {
            public const string IsMusicEnabled = "IsMusicEnabled";
            public const string IsSoundEnabled = "IsSoundEnabled";
            public const string IsHapticEnabled = "IsHapticEnabled";
            public const string SubscribeTime = "SubscribeTime";
            
            public const string LastSessionTime = "LastSessionTime";
        }
        
        public static DateTime SubscribeTime
        {
            get => GetDateTimeValue(Keys.SubscribeTime, DateTime.MinValue);
            set => SetDateTimeValue(Keys.SubscribeTime, value);
        }
        
        public static bool IsMusicEnabled
        {
            get => GetBoolValue(Keys.IsMusicEnabled, true);
            set => SetBoolValue(Keys.IsMusicEnabled, value);
        }
        
        public static bool IsSoundEnabled
        {
            get => GetBoolValue(Keys.IsSoundEnabled, true);
            set => SetBoolValue(Keys.IsSoundEnabled, value);
        }
        
        public static bool IsHapticEnabled
        {
            get => GetBoolValue(Keys.IsHapticEnabled, true);
            set => SetBoolValue(Keys.IsHapticEnabled, value);
        }
        
        public static DateTime LastSessionTime
        {
            get
            {
                return ReadTimestamp(Keys.LastSessionTime, DateTime.Now);
            }
            set
            {
                WriteTimestamp(Keys.LastSessionTime, value);
            }
        }

        private static void SetBoolValue(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        private static bool GetBoolValue(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }
        
        private static DateTime ReadTimestamp(string key, DateTime defaultValue)
        {
            long tmp = Convert.ToInt64(PlayerPrefs.GetString(key, "0"));
            if (tmp == 0)
            {
                return defaultValue;
            }
            
            return DateTime.FromBinary(tmp);
        }
        
        private static void WriteTimestamp(string key, DateTime time)
        {
            PlayerPrefs.SetString(key, time.ToBinary().ToString());
            
            PlayerPrefs.Save();
        }
        
        private static DateTime GetDateTimeValue(string key, DateTime defaultValue)
        {
            DateTime time;
            string data = PlayerPrefs.GetString(key, "");
            if (String.IsNullOrEmpty(data))
                return defaultValue;

            if (data.TryDeserializeDateTime(out time))
                return time;

            return defaultValue;
        }

        private static void SetDateTimeValue(string key, DateTime value)
        {
            PlayerPrefs.SetString(key, value.Serialize());
        }
    }
}