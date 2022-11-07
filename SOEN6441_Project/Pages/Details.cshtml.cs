using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SOEN6441_Project.Entities.Output;

namespace SOEN6441_Project.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;
        public List<Departure>? departures { get; set; }
        public List<Arrival>? arrivals { get; set; }
        public List<Flight>? flights { get; set; }
        public List<Airline>? airlines { get; set; }
        public List<Codeshared>? codeshareds { get; set; }


        public DetailsModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public void OnGet()
        {
            DBContext dBContext = DBContext.getInstance(_config);
            int flightRecordId = Convert.ToInt32(Request.Query["Id"].ToString());
            departures = new List<Departure>();
            arrivals = new List<Arrival>();
            flights = new List<Flight>();
            airlines = new List<Airline>();
            codeshareds = new List<Codeshared>();

            departures = Utility.ConvertDataTableToList<Departure>(dBContext.SelectCollection(new Departure() { FlightRecordId = flightRecordId }, new List<string>() { "FlightRecordId" }));
            arrivals = Utility.ConvertDataTableToList<Arrival>(dBContext.SelectCollection(new Arrival() { FlightRecordId = flightRecordId }, new List<string>() { "FlightRecordId" }));
            flights = Utility.ConvertDataTableToList<Flight>(dBContext.SelectCollection(new Flight() { FlightRecordId = flightRecordId }, new List<string>() { "FlightRecordId" }));
            airlines = Utility.ConvertDataTableToList<Airline>(dBContext.SelectCollection(new Airline() { FlightRecordId = flightRecordId }, new List<string>() { "FlightRecordId" }));
            codeshareds = Utility.ConvertDataTableToList<Codeshared>(dBContext.SelectCollection(new Codeshared() { FlightRecordId = flightRecordId }, new List<string>() { "FlightRecordId" }));

        }

        public ActionResult OnPostAsync()
        {
            return Page();
        }

    }
}
