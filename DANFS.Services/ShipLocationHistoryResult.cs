using System;
using DANFS.Services;

namespace DANFS.Services
{
	public class ShipLocationHistoryResult
	{
		public DateTime? PossibleStartDate { get; set;}
		public DateTime? PossibleEndDate {get; set;}
		public ShipToken ShipToken { get; set;}
		public GeocodeResultMain LocationGeocodeResult {get; set;}
		public string Location {get; set;}
		public int LocationIndex { get; set; }
	}
}

