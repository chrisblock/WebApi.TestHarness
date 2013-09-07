using System;
using System.Collections.Generic;

namespace WebApi.TestHarness.Configuration
{
	[Serializable]
	public class RouteConfiguration
	{
		public string Name { get; set; }
		public string Template { get; set; }
		public IEnumerable<RouteConfigurationParameter> DefaultParameters { get; set; }
	}
}
