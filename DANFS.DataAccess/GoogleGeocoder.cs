using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DANFS.Services;

namespace DANFS.DataAccess
{
    class GoogleGeocoder
    {
        public async Task<GeocodeResultMain> DoGecode(string address)
        {
            var rawJSON = await DoGeocodeRawJSON(address);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<GeocodeResultMain>(rawJSON);
        }

        public async Task<string> DoGeocodeRawJSON(string address)
        {
            var client = new HttpClient();

            var uriString = string.Format("http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=false", Uri.EscapeDataString(address));

            Uri uri = new Uri(uriString);

           return await client.GetStringAsync(uri);           
        }
    }
}
