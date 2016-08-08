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

		public async Task<string> GetDisplayableHTMLForShip(string shipID)
		{
			//First load the xml from Amazon S3 (for now, probably SQLite later).
			/*var client = new HttpClient();
			var stream = await client.GetStreamAsync($"http://s3-us-west-2.amazonaws.com/danfs/{shipID}.xml");

			//Load the XSLT transform we will use to translate to HTML.
			var doc = XDocument.Load(stream);

			//Transform
			//Return the HTML string.
			return ShipXMLTransformer.GetHTML(doc);*/
			var connection = new SQLite.Net.SQLiteConnection(
				TinyIoC.TinyIoCContainer.Current.Resolve<ISQLitePlatform>(),
				TinyIoC.TinyIoCContainer.Current.Resolve<IFolderProvider>().MasterDatabasePath);

			var query = connection.Table<danfs_ships>().Where(r => r.id == shipID).FirstOrDefault();

			return ShipXMLTransformer.GetHTML(XDocument.Parse(query.history));

		}

		private Dictionary<string, ShipToken> _shipLookup;

		private List<ShipToken> _allShipCache;

		public async Task<Dictionary<string, ShipToken>> GetShipLookupTable()
		{
			if (_shipLookup == null)
			{
				await GetAllShips();
			}
			return _shipLookup;
		}

		public async Task<System.Collections.Generic.IList<ShipToken>> GetAllShips() {
			/*var client = new HttpClient ();
			var stream = await client.GetStreamAsync ("http://s3-us-west-2.amazonaws.com/danfs/manifest.json");

			var serializer = new JsonSerializer();

			using (var sr = new StreamReader(stream))
			using (var jsonTextReader = new JsonTextReader(sr))
			{
				var allShipsResponse = serializer.Deserialize<List<ShipToken>>(jsonTextReader);

				return allShipsResponse.Cast<ShipToken> ().ToList();
			}*/

			if (_allShipCache == null || _shipLookup == null)
			{

				var connection = new SQLite.Net.SQLiteConnection(
					TinyIoC.TinyIoCContainer.Current.Resolve<ISQLitePlatform>(),
					TinyIoC.TinyIoCContainer.Current.Resolve<IFolderProvider>().MasterDatabasePath);

				var query = connection.Table<danfs_ships>().Select(r => new ShipToken() { ID = r.id, Title = r.title });

				_allShipCache = query.ToList();
				_shipLookup = new Dictionary<string, ShipToken>(10000);
				foreach (var entry in _allShipCache)
				{
					_shipLookup.Add(entry.ID, entry);
				}
			}
			return _allShipCache;
		}

		public System.Collections.Generic.IList<ShipToken> GetAllShipChunk(int start, int length)
		{
			var connection = new SQLite.Net.SQLiteConnection(
				TinyIoC.TinyIoCContainer.Current.Resolve<ISQLitePlatform>(),
				TinyIoC.TinyIoCContainer.Current.Resolve<IFolderProvider>().MasterDatabasePath);

			var query = connection.Table<danfs_ships>().Select(r => new ShipToken() { ID = r.id, Title = r.title }).Skip(start).Take(length);

			return query.ToList();
		}

		public int GetAllShipCounts()
		{
			var connection = new SQLite.Net.SQLiteConnection(
				TinyIoC.TinyIoCContainer.Current.Resolve<ISQLitePlatform>(),
				TinyIoC.TinyIoCContainer.Current.Resolve<IFolderProvider>().MasterDatabasePath);

			var getCountCommand = connection.CreateCommand("select COUNT(*) from danfs_ships");
			return getCountCommand.ExecuteScalar<int>();
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

		public List<string> GetUniqueLocationList()
		{
			/*var connection = new SQLite.Net.SQLiteConnection(
				TinyIoC.TinyIoCContainer.Current.Resolve<ISQLitePlatform>(),
				TinyIoC.TinyIoCContainer.Current.Resolve<IFolderProvider>().MapDatabasePath);



			var query = connection.Table<shipLocationDate>().OrderBy(r => r.locationname);

			return query.Select(r => r.locationname).Distinct().ToList();*/

			Stream stream = TinyIoC.TinyIoCContainer.Current.Resolve<IFolderProvider>().UniqueLocationsFileStream;

			var uniqueLocations = new List<string>();

			using (var reader = new System.IO.StreamReader(stream))
			{
				var nextLine = reader.ReadLine();
				while (!string.IsNullOrEmpty(nextLine))
				{
					uniqueLocations.Add(nextLine);
					nextLine = reader.ReadLine();
				}
			}

			uniqueLocations.Sort();

			return uniqueLocations;
		}

		public List<shipLocationDate> GetShipListByLocation(string locationName)
		{
			var connection = new SQLite.Net.SQLiteConnection(
				TinyIoC.TinyIoCContainer.Current.Resolve<ISQLitePlatform>(),
				TinyIoC.TinyIoCContainer.Current.Resolve<IFolderProvider>().MapDatabasePath);



			var query = connection.Table<shipLocationDate>().Where(r => r.locationname == locationName);

			var result = query.ToList();

			return result.OrderBy(r => r.OrderedDate).ToList();
		}

		public List<shipLocationDate> GetShipListByMultipleLocations(List<string> locationNames)
		{
			var connection = new SQLite.Net.SQLiteConnection(
				TinyIoC.TinyIoCContainer.Current.Resolve<ISQLitePlatform>(),
				TinyIoC.TinyIoCContainer.Current.Resolve<IFolderProvider>().MapDatabasePath);


			var query = connection.Table<shipLocationDate>().Where(r => locationNames.Contains(r.locationname));

			var result = query.ToList();

			return result.OrderBy(r => r.OrderedDate).ToList();
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
				var possibleLocation = connection.Table<locationJSON> ().FirstOrDefault (l => l.name == shipLocation.locationname);

				if (possibleLocation == null)
				{
					//We purposely skip some locations when processing. May want to re-think that!
					continue;
				}

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
					LocationGeocodeResult = possibleLocation != null ? JsonConvert.DeserializeObject<GeocodeResultMain> (possibleLocation.geocodeJSON) : null,
					LocationGuid = shipLocation.locationguid
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

				

		public class locationJSON
		{
			public string name {get; set;}
			public string geocodeJSON { get; set; }
		}

		public async Task<IList<string>> GetLocationsForShip(ShipToken ship) {
			//Take the ID out of ship, load the XML, use XDocument to return all the internal strings of the LOCATION XML elements.
			/*var client = new HttpClient ();
			var stream = await client.GetStreamAsync (string.Format("http://s3-us-west-2.amazonaws.com/danfs/{0}.xml", ship.ID));
			var document = XDocument.Load (stream);
			return document.Descendants ("LOCATION").Select (e => e.Value).ToList ();*/

			//Get these from the SQLite database now, instead of processing the XML.
			using (var connection = new SQLite.Net.SQLiteConnection(
				TinyIoC.TinyIoCContainer.Current.Resolve<ISQLitePlatform>(),
				TinyIoC.TinyIoCContainer.Current.Resolve<IFolderProvider>().MapDatabasePath))
			{
				var table = connection.Table<shipLocationDate>();

				var query = table.Where(r => r.shipID == ship.ID).OrderBy(r => r.startdate);

				return query.Select(r => r.locationname).ToList();
			}
		}

		#endregion
	}
}

