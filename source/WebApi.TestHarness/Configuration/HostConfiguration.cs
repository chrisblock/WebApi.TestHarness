using System;

namespace WebApi.TestHarness.Configuration
{
	[Serializable]
	public class HostConfiguration
	{
		public RouteConfigurationTable RouteTable { get; set; }
		public Type DependencyResolverType { get; set; }
	}
}
