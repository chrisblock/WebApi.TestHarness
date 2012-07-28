using System;
using System.Collections.Generic;

namespace WebApi.TestHarness
{
	[Serializable]
	public class RouteCounfiguration
	{
		public string Name { get; set; }
		public string Template { get; set; }
		public IEnumerable<RouteConfigurationParameter> DefaultParameters { get; set; }
	}
}
