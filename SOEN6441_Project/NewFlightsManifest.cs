using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SOEN6441_Project.Entities.Output;
using System.Text.Json;
using SOEN6441_Project.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;

namespace SOEN6441_Project
{
    public class NewFlightsManifest : INewFlightsManifest
    {
        private ManifestResponseEntity responseEntity;
        private readonly IConfiguration _config;
        private List<IDatabaseSubscriber> observers = new List<IDatabaseSubscriber>();

        public NewFlightsManifest(IConfiguration config)
        {
            _config = config;
        }

        public void addObserver(IDatabaseSubscriber observer)
        {
            observers.Add(observer);
        }

        public void removeObserver(IDatabaseSubscriber observer)
        {
            observers.Remove(observer);
        }

        public void notifyObservers()
        {
            for (int i = 0; i < observers.Count(); i++)
            {
                observers[i].update();
            }
        }

        public void GetNewFlightsManifest()
        {
            string uri = _config.GetValue<string>("InternalAPI:URI");
            string apiKey = _config.GetValue<string>("InternalAPI:YOUR_ACCESS_KEY");
            uri = uri.Replace("YOUR_ACCESS_KEY", apiKey);


            ManifestResponseEntity responseEntity = new ManifestResponseEntity();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(uri).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response body.
                        responseEntity = response.Content.ReadAsAsync<ManifestResponseEntity>().Result;
                        if (responseEntity != null)
                            this.responseEntity = responseEntity;
                        else
                            this.responseEntity = new ManifestResponseEntity();
                    }
                    else
                        this.responseEntity = new ManifestResponseEntity();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            this.notifyObservers();
        }

        public ManifestResponseEntity GetManifestResponse()
        {
            return this.responseEntity;
        }

    }
}

