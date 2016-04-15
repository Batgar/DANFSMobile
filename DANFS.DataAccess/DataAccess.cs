using System;
using DANFS.Services;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SQLite.Net.Interop;

namespace DANFS.DataAccess
{
	public class DataAccess : IDataAccess
	{
		public DataAccess ()
		{
		}

		#region IDataAccess implementation

		public async Task<System.Collections.Generic.IList<IShipToken>> GetAllShips() {
			var client = new HttpClient ();
			var stream = await client.GetStreamAsync ("http://s3-us-west-2.amazonaws.com/danfs/manifest.json");

			var serializer = new JsonSerializer();

			using (var sr = new StreamReader(stream))
			using (var jsonTextReader = new JsonTextReader(sr))
			{
				var allShipsResponse = serializer.Deserialize<List<ShipToken>>(jsonTextReader);

				return allShipsResponse.Cast<IShipToken> ().ToList();
			}
		}

		public async Task<List<GeocodeResultMain>> GetRawGeolocationsForShip(IShipToken ship)
		{
			var connection = new SQLite.Net.SQLiteConnection (
				TinyIoC.TinyIoCContainer.Current.Resolve<ISQLitePlatform>(),
				TinyIoC.TinyIoCContainer.Current.Resolve<IFolderProvider>().MapDatabasePath);

			var locations = await GetLocationsForShip (ship);

			List<GeocodeResultMain> mainGeocodeResults = new List<GeocodeResultMain> ();

			foreach (var location in locations) {
				List<GeocodeResultMain> results = new List<GeocodeResultMain> ();
				var query = connection.Table<locationJSON> ().Where (l => l.name == location);
				foreach (var locationJSONEntry in query) {
					mainGeocodeResults.Add(JsonConvert.DeserializeObject<GeocodeResultMain>(locationJSONEntry.geocodeJSON));
				}
			}

			return mainGeocodeResults;
		}

		public class locationJSON
		{
			public string name {get; set;}
			public string geocodeJSON { get; set; }
		}


		public async Task<IList<string>> GetLocationsForShip(IShipToken ship) {
			//Take the ID out of ship, load the XML, use XDocument to return all the internal strings of the LOCATION XML elements.
			var client = new HttpClient ();
			var stream = await client.GetStreamAsync (string.Format("http://s3-us-west-2.amazonaws.com/danfs/{0}.xml", ship.ID));
			var document = XDocument.Load (stream);
			return document.Descendants ("LOCATION").Select (e => e.Value).ToList ();
		}

		#endregion
	}
}

