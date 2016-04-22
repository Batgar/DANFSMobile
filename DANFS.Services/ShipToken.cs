using System;
using DANFS.Services;
using Newtonsoft.Json;

namespace DANFS.Services
{
	public struct ShipToken
	{
		
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

		public string Serialize()
		{
			return JsonConvert.SerializeObject(this);
		}

		public static ShipToken Deserialize(string value)
		{
			return JsonConvert.DeserializeObject<ShipToken> (value);
		}

		public static ShipToken Empty = new ShipToken (){ ID = string.Empty, Title = string.Empty };
	}
}

