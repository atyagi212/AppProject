using System;
namespace SOEN6441_Project
{
    public class Constants
    {
        public int currentFlightId;

        public enum SubTables
        {
            DEPARTURE,
            ARRIVAL,
            AIRLINE,
            FLIGHT,
            CODESHARED
        }

        public void setCurrentFlightId(int value)
        {
            this.currentFlightId = value;
        }

        public int getCurrentFlightId()
        {
            return this.currentFlightId;
        }
    }
}

