using Microsoft.VisualBasic;
using SOEN6441_Project.Entities.Output;
using SOEN6441_Project.Interfaces;
using System.Data;

namespace SOEN6441_Project
{
    public class PublishFlights:IDatabaseSubscriber
    {
        public NewFlightsManifest _newFlightsManifest;
        private ManifestResponseEntity _manifestResponseEntity;
        private IConfiguration _config;

        public PublishFlights(NewFlightsManifest newFlightsManifest, IConfiguration config)
        {
            this._newFlightsManifest = newFlightsManifest;
            this._config = config;
            this._newFlightsManifest.addObserver(this);
        }

        public void update()
        {
            this._manifestResponseEntity = _newFlightsManifest.GetManifestResponse();
            //CreateNewFlightData(this._manifestResponseEntity);
        }

        public void CreateNewFlightData(ManifestResponseEntity response)
        {
            if (response != null && response.data.Count > 0)
            {
                ResetFlightsData();
                Constants constants = new Constants();
                constants.setCurrentFlightId(GetLatestFlightId());

                foreach (var record in response.data)
                {
                    record.Id = constants.getCurrentFlightId();
                    DBContext dbContext = DBContext.getInstance(_config);
                    dbContext.InsertCollection(record);
                    FlightRecords tempRecord = ParseFlightData(record);
                    InsertComplexTables(tempRecord, dbContext);
                    constants.setCurrentFlightId(record.Id + 1);
                }
            }
        }

        public void ResetFlightsData()
        {
            DBContext dBContext = DBContext.getInstance(_config);
            dBContext.DeleteAllCollection(new Codeshared());
            dBContext.DeleteAllCollection(new Flight());
            dBContext.DeleteAllCollection(new Airline());
            dBContext.DeleteAllCollection(new Departure());
            dBContext.DeleteAllCollection(new Arrival());
            dBContext.DeleteAllCollection(new FlightRecords());
        }

        public int GetLatestFlightId()
        {
            DBContext context = DBContext.getInstance(_config);
            IEnumerable<DataRow> rows = context.SelectAllCollection(new FlightRecords());
            if (rows != null && rows.Count() > 0)
            {
                DataTable dt = rows.CopyToDataTable();
                return Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["Id"].ToString()) + 1;
            }
            else
                return 1;
        }

        public void InsertComplexTables(FlightRecords tableType, DBContext context)
        {
            if (tableType.flight != null)
            {
                context.InsertCollection(tableType.flight);
                if (tableType.flight.codeshared != null)
                    context.InsertCollection(tableType.flight.codeshared);
            }

            if (tableType.arrival != null)
                context.InsertCollection(tableType.arrival);

            if (tableType.departure != null)
                context.InsertCollection(tableType.departure);

            if (tableType.airline != null)
                context.InsertCollection(tableType.airline);
        }

        public FlightRecords ParseFlightData(FlightRecords flightRecord)
        {
            if (flightRecord != null)
            {
                if (flightRecord.departure != null)
                    flightRecord.departure.FlightRecordId = flightRecord.Id;

                if (flightRecord.arrival != null)
                    flightRecord.arrival.FlightRecordId = flightRecord.Id;

                if (flightRecord.airline != null)
                    flightRecord.airline.FlightRecordId = flightRecord.Id;

                if (flightRecord.flight != null)
                {
                    flightRecord.flight.FlightRecordId = flightRecord.Id;

                    if (flightRecord.flight.codeshared != null)
                        flightRecord.flight.codeshared.FlightRecordId = flightRecord.Id;
                }
            }
            return flightRecord;
        }

    }
}
