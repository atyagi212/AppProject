using SOEN6441_Project.Entities.Output;

namespace SOEN6441_Project.Interfaces
{
    public interface IDatabaseSubscriber
    {
        void update();

        void CreateNewFlightData(ManifestResponseEntity response);

        void ResetFlightsData();

        int GetLatestFlightId();

        void InsertComplexTables(FlightRecords tableType, DBContext context);

        FlightRecords ParseFlightData(FlightRecords flightRecord);
    }
}
