using System;
using DANFS.Services;
using Newtonsoft.Json;

namespace DANFS.DataAccess
{
	public struct ShipToken : IShipToken
	{
		#region IShipToken implementation
		[JsonProperty(PropertyName="id")]
		public string ID {
			get ;
			internal set;
		}

		[JsonProperty(PropertyName="title")]
		public string Title {
			get ;
			internal set;
		}
		#endregion
	}
}

