using System;

namespace Framework.Core.Common
{
    public class UTC
    {
        /// <summary>
        /// DateTime 转UTC时间
        /// </summary>
        /// <param name="Time"></param>
        /// <returns></returns>
        public static long ConvertDateTimeLong(DateTime Time)//DateTime time = System.DateTime.UtcNow;
        {
            double doubleResult = 0;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime endTime = Time;
            doubleResult = (endTime - startTime).TotalSeconds;
            return (long)(doubleResult);
        }

        /// <summary>
        /// UTC转 DateTime
        /// </summary>
        /// <param name="UTCTime"></param>
        /// <returns></returns>
        public static DateTime ConvertLongDateTime(long UTCTime)
        {
            DateTime time = DateTime.MinValue;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            time = startTime.AddSeconds(UTCTime);
            return time;
        }
    }
}
