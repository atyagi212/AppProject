using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using ManifestInformation.Entities.Input;
using ManifestInformation.Entities.Output;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ManifestInformation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManifestController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ManifestController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// API to get the manifest data for input combination of airport code, flight number and date
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetManifest()
        {
            string uri = _config.GetValue<string>("InternalAPI:URI");
            string apiKey = _config.GetValue<string>("InternalAPI:YOUR_ACCESS_KEY");
            uri = uri.Replace("YOUR_ACCESS_KEY", apiKey);


            ManifestResponseEntity responseEntity = new ManifestResponseEntity();
            try
            {
                //   return BadRequest("Invalid Input fields");

                using (HttpClient client = new HttpClient())
                {
                    // Add an header for JSON format.
                    // client.DefaultRequestHeaders.Add(headerKey, headerValue);

                    HttpResponseMessage response = client.GetAsync(uri).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response body.
                        responseEntity = response.Content.ReadAsAsync<ManifestResponseEntity>().Result;
                        var settings = new JsonSerializerOptions
                        {
                            IgnoreReadOnlyProperties = true,
                            WriteIndented = true
                        };
                        string json = JsonConvert.SerializeObject(responseEntity);
                        return Content(JObject.Parse(json).ToString(), "application/json");

                    }
                    else
                    {
                        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    }
                }
            }

            catch (Exception)
            {
                return StatusCode(500, new JsonResult("Service is Unavilable, please try again later."));
            }

            var options = new JsonSerializerOptions
            {
                IgnoreReadOnlyProperties = true,
                WriteIndented = true
            };
            return Ok(new JsonResult(new ManifestResponseEntity(), options));
        }

    }
}
