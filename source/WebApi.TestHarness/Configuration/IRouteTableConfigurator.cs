using System;

namespace WebApi.TestHarness.Configuration
{
	public interface IRouteTableConfigurator
	{
		IRouteTableConfigurator WithBase(string baseUrl);
		IRouteTableConfigurator WithBase(Uri baseUri);
		IRouteTableConfigurator AddRoute(RouteConfiguration routeConfiguration);
		IRouteTableConfigurator AddRoute(string name, string template, params RouteConfigurationParameter[] defaultParameters);
		IRouteTableConfigurator AddRoute(Func<IRouteConfigurator, IRouteConfigurator> configure);
	}
}
