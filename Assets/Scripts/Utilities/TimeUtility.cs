using System;

namespace TimeUtility
{
    public class TimeStamp
    {
        // Date time of the beginning of the current game session
        public static DateTime GameStart
        {
            get
            {
                // Get elapsed seconds since game started
                double elapsedSeconds = UnityEngine.Time.realtimeSinceStartup;

                // Subtract elapsed time from now to get game start datetime
                return DateTime.UtcNow.AddSeconds(-1 * elapsedSeconds);
            }
        }

    }

}
