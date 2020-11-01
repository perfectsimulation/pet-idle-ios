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

    public class TimeInterval
    {
        // Check if there is an overlap during datetime sets of A and B
        public static bool HasOverlap(
            DateTime ArrivalA, DateTime DepartureA,
            DateTime ArrivalB, DateTime DepartureB)
        {
            // Return true if the sets share a common datetime
            if (ArrivalA.Equals(ArrivalB) ||
                // A: <----------------->
                // B: <----------->

                ArrivalA.Equals(DepartureB) ||
                // A:       <----------->
                // B: <----->

                DepartureA.Equals(ArrivalB) ||
                // A: <----->
                // B:       <----------->

                DepartureA.Equals(DepartureB))
                // A: <----------------->
                // B:       <----------->
            {
                return true;
            }

            // Determine if arrival A occurs before arrival B
            if (ArrivalA < ArrivalB)
            {
                // Confirm overlap if arrival B occurs before departure A
                // A: <------------->
                // B:      <------------>
                return ArrivalB < DepartureA;
            }

            // Confirm overlap if arrival A occurs before departure B
            // A:      <------------>
            // B: <------------->
            return ArrivalA < DepartureB;
        }
    }

}
