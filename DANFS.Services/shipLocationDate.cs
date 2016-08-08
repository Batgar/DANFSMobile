using System;
namespace DANFS.Services
{
	public class shipLocationDate
	{
		public string shipID { get; set; }
		public string locationname { get; set; }
		public string startdate { get; set; }
		public string enddate { get; set; }
		public string locationguid { get; set; }

		public string OrderedDate
		{
			get
			{
				if (!string.IsNullOrEmpty(startdate))
				{
					try
					{
						var dateTime = DateTime.Parse(startdate);
						return dateTime.ToString("yyyy-MM-dd");
					}
					catch
					{
						
					}
				}

				return DateTime.MinValue.ToString("yyyy-MM-dd");
			}
		}

		public string shiplocationdatetype { get; set;}


	}
}

