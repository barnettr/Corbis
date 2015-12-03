using System;

namespace Corbis.Web.Entities
{
	[Serializable]
	public class OfficeContactInfo
	{
		public string ContactName { get; set; }
		public string EmailAddress { get; set; }
		public string OfficeName { get; set; }
		public string OfficeFaxNumber { get; set; }
		public string OfficePhoneNumber { get; set; }
		public string OfficeAddress { get; set; }
		public string DefaultOfficeInfo { get; set; }
		public bool UseDefaultOfficeInfo { get; set; }
		public bool IsOtherLocations { get; set; }
	}
}
