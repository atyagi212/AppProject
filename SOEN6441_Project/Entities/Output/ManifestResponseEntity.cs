namespace SOEN6441_Project.Entities.Output
{
    public class ManifestResponseEntity
    {
        
        public Pagination pagination { get; set; }
        public List<FlightRecords> data { get; set; }
    }

    public class Airline
    {
        public int Id { get; set; }
        public int FlightRecordId { get; set; }
        public string name { get; set; }
        public string iata { get; set; }
        public string icao { get; set; }
    }

    public class Arrival
    {
        public int Id { get; set; }
        public int FlightRecordId { get; set; }
        public string airport { get; set; }
        public string timezone { get; set; }
        public string iata { get; set; }
        public string icao { get; set; }
        public string terminal { get; set; }
        public string gate { get; set; }
        public string baggage { get; set; }
        public DateTime scheduled { get; set; }
        public DateTime estimated { get; set; }
    }

    public class Codeshared
    {
        public int Id { get; set; }
        public int FlightRecordId { get; set; }
        public string airline_name { get; set; }
        public string airline_iata { get; set; }
        public string airline_icao { get; set; }
        public string flight_number { get; set; }
        public string flight_iata { get; set; }
        public string flight_icao { get; set; }
    }

    public class FlightRecords
    {
        public int Id { get; set; }
        public string flight_date { get; set; }
        public string flight_status { get; set; }
        public Departure departure { get; set; }
        public Arrival arrival { get; set; }
        public Airline airline { get; set; }
        public Flight flight { get; set; }
    }

    public class Departure
    {
        public int Id { get; set; }
        public int FlightRecordId { get; set; }
        public string airport { get; set; }
        public string timezone { get; set; }
        public string iata { get; set; }
        public string icao { get; set; }
        public string terminal { get; set; }
        public string gate { get; set; }
        public int? delay { get; set; }
        public DateTime scheduled { get; set; }
        public DateTime estimated { get; set; }
    }

    public class Flight
    {
        public int Id { get; set; }
        public int FlightRecordId { get; set; }
        public string number { get; set; }
        public string iata { get; set; }
        public string icao { get; set; }
        public Codeshared codeshared { get; set; }
    }

    public class Pagination
    {
        public int limit { get; set; }
        public int offset { get; set; }
        public int count { get; set; }
        public int total { get; set; }
    }
}
