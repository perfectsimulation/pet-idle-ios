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

}
