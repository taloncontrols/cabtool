using System;
using System.Collections.Generic;
using System.Text;

namespace CabSvr.Fingerprint.Dtos
{
	public class Reader
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string SerialNumber { get; set; }
		public string Manufacturer { get; set; }
		public string Model { get; set; }
	}
}
