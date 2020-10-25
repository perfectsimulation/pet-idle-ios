using System;

namespace TimeUtility
{
    // Enable serialization of DateTime
    [Serializable]
    public struct SerializedDateTime
    {
        // String representation of a DateTime
        public string stringValue;

        public static implicit operator DateTime(SerializedDateTime serializedDateTime)
        {
            return Convert.ToDateTime(serializedDateTime.stringValue);
        }

        public static implicit operator SerializedDateTime(DateTime dateTime)
        {
            SerializedDateTime serializedDateTime = new SerializedDateTime
            {
                stringValue = dateTime.ToString()
            };
            return serializedDateTime;
        }

    }

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
