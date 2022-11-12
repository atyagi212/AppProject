using System.Data;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using SOEN6441_Project.Entities.Output;

namespace SOEN6441_Project.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;
        public List<FlightRecords>? flightRecords { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public void OnGet()
        {
            DataMapper dBContext = DataMapper.getInstance(_config);
            flightRecords = new List<FlightRecords>();
            var records = dBContext.SelectAllCollection(new FlightRecords());
            if(records.Count()>0)
            {
                flightRecords = Utility.ConvertDataTableToList<FlightRecords>(records);
            }
        }

        public ActionResult OnPost()
        {
            NewFlightsManifest newFlightsManifest = new NewFlightsManifest(_config);
            PublishFlights observer1 = new PublishFlights(newFlightsManifest, _config);
            newFlightsManifest.GetNewFlightsManifest();
            return RedirectToPage("./Index");
        }
    }
}