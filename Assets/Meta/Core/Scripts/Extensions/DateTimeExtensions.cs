using System;
using System.Globalization;
using System.Text;

namespace Core
{
    public static class DateTimeExtensions
    {
        private const string SerializationDateFormat = "yyyy-MM-dd HH:mm:ss";

        public static string Serialize(this DateTime dateTime)
        {
            return dateTime.ToString(SerializationDateFormat);
        }

        public static bool TryDeserializeDateTime(this string data, out DateTime result)
        {
            return DateTime.TryParseExact(data, SerializationDateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out result);
        }

        public static TimeSpan ToTimeSpan(this DateTime date)
        {
            return TimeSpan.FromTicks(date.Ticks);
        }

        public static string FormatTimer(this TimeSpan span, bool applyZeros)
        {
            //string dayFormat = "{0}"; // Strings.TimerDayFormat
            string hourFormat = "{0}:"; // Strings.TimerHourFormat
            string minuteFormat = "{0}:"; // Strings.TimerMinuteFormat
            string secondFormat = "{0}"; // Strings.TimerSecondFormat

            StringBuilder result = new StringBuilder();

            int days = span.Days;
            int hours = span.Hours;
            int min = span.Minutes;
            int sec = span.Seconds;

            if (days > 0)
            {
                hours += 24 * days;
            }

            if (hours > 0)
            {
                if (hours < 10)
                {
                    result.Append("0");
                }

                result.AppendFormat(hourFormat, hours);

                if (applyZeros || min > 0)
                {
                    if (min < 10)
                    {
                        result.Append("0");
                    }

                    result.AppendFormat(minuteFormat, min);
                }

                if (applyZeros || sec > 0)
                {
                    if (sec < 10)
                    {
                        result.Append("0");
                    }

                    result.AppendFormat(secondFormat, sec);
                }

            }
            else if (min > 0)
            {
                if (min < 10)
                {
                    result.Append("0");
                }

                result.AppendFormat(minuteFormat, min);

                if (applyZeros || sec > 0)
                {
                    if (sec < 10)
                    {
                        result.Append("0");
                    }

                    result.AppendFormat(secondFormat, sec);
                }
            }
            else if (sec > 0)
            {
                if (sec < 10)
                {
                    result.Append("0");
                }

                result.AppendFormat(secondFormat, sec);
            }

            return result.ToString();
        }
    }
}