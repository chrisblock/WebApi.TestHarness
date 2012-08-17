using System;
using System.Linq;

namespace WebApi.TestHarness.Configuration
{
	[Serializable]
	public class HostConfiguration
	{
		public RouteConfigurationTable RouteTable { get; set; }
	}
}
