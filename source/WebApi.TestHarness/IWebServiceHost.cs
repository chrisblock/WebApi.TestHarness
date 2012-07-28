using System;

namespace WebApi.TestHarness
{
	public interface IWebServiceHost : IDisposable
	{
		void Start(RouteConfigurationTable routeConfigurationTable);
	}
}
