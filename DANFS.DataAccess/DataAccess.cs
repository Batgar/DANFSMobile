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
using System.Globalization;

namespace DANFS.DataAccess
{
	public class DataAccess : IDataAccess
	{
		public DataAccess ()
		{
		}

		#region IDataAccess implementation


		public async Task<System.Collections.Generic.IList<ShipToken>> GetAllShips() {
			var client = new HttpClient ();
			var stream = await client.GetStreamAsync ("http://s3-us-west-2.amazonaws.com/danfs/manifest.json");

			var serializer = new JsonSerializer();

			using (var sr = new StreamReader(stream))
			using (var jsonTextReader = new JsonTextReader(sr))
			{
				var allShipsResponse = serializer.Deserialize<List<ShipToken>>(jsonTextReader);

				return allShipsResponse.Cast<ShipToken> ().ToList();
			}
		}

		public IEnumerable<shipdate> GetTodayInNavyHistoryByYear(string year)
		{
			var today = DateTime.Now;

			var day = Convert.ToString(today.Day);

			var month = today.ToString("MMMM", new CultureInfo("en-US"));

			var connection = new SQLite.Net.SQLiteConnection(
				TinyIoC.TinyIoCContainer.Current.Resolve<ISQLitePlatform>(),
				TinyIoC.TinyIoCContainer.Current.Resolve<IFolderProvider>().DateDatabasePath);

			var query = connection.Table<shipdate>().Where(r => r.day == day && r.month == month && r.year == year);

			return query;
		}

		public IEnumerable<string> GetTodayInNavyHistoryYearSections()
		{
			var today = DateTime.Now;

			var day = Convert.ToString(today.Day);

			var month = today.ToString("MMMM", new CultureInfo("en-US"));
			
			var connection = new SQLite.Net.SQLiteConnection(
				TinyIoC.TinyIoCContainer.Current.Resolve<ISQLitePlatform>(),
				TinyIoC.TinyIoCContainer.Current.Resolve<IFolderProvider>().DateDatabasePath);

			var query = connection.Table<shipdate>().Where(r => r.day == day && r.month == month).OrderBy(r => r.year);

			return query.Select(r => r.year).Distinct();
		}

		public IList<shipdate> GetTodayInNavyHistory()
		{
			var today = DateTime.Now;

			var day = Convert.ToString(today.Day);

			var month = today.ToString("MMMM", new CultureInfo("en-US"));

			var connection = new SQLite.Net.SQLiteConnection(
				TinyIoC.TinyIoCContainer.Current.Resolve<ISQLitePlatform>(),
				TinyIoC.TinyIoCContainer.Current.Resolve<IFolderProvider>().DateDatabasePath);

			var query = connection.Table<shipdate>().Where(r => r.day == day && r.month == month).OrderBy(r => r.year);

			return query.ToList();
		}



		public async Task<List<ShipLocationHistoryResult>> GetRawGeolocationsForShip(ShipToken ship)
		{
			var connection = new SQLite.Net.SQLiteConnection (
				TinyIoC.TinyIoCContainer.Current.Resolve<ISQLitePlatform>(),
				TinyIoC.TinyIoCContainer.Current.Resolve<IFolderProvider>().MapDatabasePath);
			

			List<ShipLocationHistoryResult> mainShipLocationResults = new List<ShipLocationHistoryResult> ();

			var query = connection.Table<shipLocationDate> ().Where (r => r.shipID == ship.ID).OrderBy (r => r.startdate);

			int locationIndex = 1;

			foreach (var shipLocation in query) {
				var possibleLocation = connection.Table<locationJSON> ().First (l => l.name == shipLocation.locationname);

				DateTime startDate;
				DateTime endDate;

				var hasStartDate = string.IsNullOrEmpty(shipLocation.startdate) ? false : DateTime.TryParse (shipLocation.startdate, out startDate);
				var hasEndDate = string.IsNullOrEmpty(shipLocation.enddate) ? false : DateTime.TryParse (shipLocation.enddate, out endDate);

				mainShipLocationResults.Add(new ShipLocationHistoryResult()
				{
					Location = shipLocation.locationname,
					PossibleEndDate = hasStartDate ? startDate : default(DateTime),
					PossibleStartDate = hasEndDate ? endDate : default(DateTime),
					ShipToken = ship,
					LocationIndex = locationIndex++,
					LocationGeocodeResult = possibleLocation != null ? JsonConvert.DeserializeObject<GeocodeResultMain> (possibleLocation.geocodeJSON) : null
				});

			/*foreach (var location in locations) {
				List<GeocodeResultMain> results = new List<GeocodeResultMain> ();
				var query = connection.Table<locationJSON> ().Where (l => l.name == location);
				foreach (var locationJSONEntry in query) {
					mainGeocodeResults.Add(JsonConvert.DeserializeObject<GeocodeResultMain>(locationJSONEntry.geocodeJSON));
				}*/
			}

			return mainShipLocationResults;
		}

				public class shipLocationDate
				{
					public string shipID {get; set;}
					public string locationname {get; set;}
					public string startdate {get; set;}
					public string enddate {get; set;}
				}

		public class locationJSON
		{
			public string name {get; set;}
			public string geocodeJSON { get; set; }
		}





		public async Task<IList<string>> GetLocationsForShip(ShipToken ship) {
			//Take the ID out of ship, load the XML, use XDocument to return all the internal strings of the LOCATION XML elements.
			var client = new HttpClient ();
			var stream = await client.GetStreamAsync (string.Format("http://s3-us-west-2.amazonaws.com/danfs/{0}.xml", ship.ID));
			var document = XDocument.Load (stream);
			return document.Descendants ("LOCATION").Select (e => e.Value).ToList ();
		}

		#endregion
	}
}

