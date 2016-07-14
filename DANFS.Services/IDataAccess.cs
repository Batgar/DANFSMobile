using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DANFS.Services
{
	public interface IDataAccess
	{
		Task<IList<ShipToken>> GetAllShips();

		Task<IList<string>> GetLocationsForShip(ShipToken ship);

		Task<List<ShipLocationHistoryResult>> GetRawGeolocationsForShip (ShipToken ship);

		IEnumerable<string> GetTodayInNavyHistoryYearSections();

		IEnumerable<shipdate> GetTodayInNavyHistoryByYear(string year);

		IList<shipdate> GetTodayInNavyHistory();

	}

	public class GeocodeResultMain
	{
		[JsonProperty(PropertyName = "results")]
		public List<GeocodeResult> Results { get; set; }

		[JsonProperty(PropertyName = "status")]
		public string Status { get; set; }
	}


	public class GeocodeResult
	{
		[JsonProperty(PropertyName = "address_components")]
		public List<GeocodeAddressComponent> AddressComponents { get; set; }

		[JsonProperty(PropertyName = "formatted_address")]
		public string FormattedAddress { get; set; }

		[JsonProperty(PropertyName = "geometry")]
		public GeocodeGeometry Geometry { get; set; }

		[JsonProperty(PropertyName = "place_id")]
		public string PlaceID { get; set; }

		[JsonProperty(PropertyName = "types")]
		public List<string> Types { get; set; }
	}

	public class GeocodeGeometry
	{
		[JsonProperty(PropertyName ="bounds")]
		public Dictionary<string, GeocodeLatLong> Bounds { get; set; }

		[JsonProperty(PropertyName = "location_type")]
		public string LocationType { get; set; }

		[JsonProperty(PropertyName = "viewport")]
		public Dictionary<string, GeocodeLatLong> Viewport { get; set; }
	}



	public class GeocodeLatLong
	{
		[JsonProperty(PropertyName ="lat")]
		public double Lat { get; set; }

		[JsonProperty(PropertyName = "lng")]
		public double Long { get; set; }
	}

	public class GeocodeAddressComponent
	{
		[JsonProperty(PropertyName ="long_name")]
		public string LongName { get; set; }

		[JsonProperty(PropertyName = "short_name")]
		public string ShortName { get; set; }

		[JsonProperty(PropertyName = "types")]
		public List<string> Types { get; set; }
	}
}

