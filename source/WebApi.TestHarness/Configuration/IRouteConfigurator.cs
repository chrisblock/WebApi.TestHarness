namespace WebApi.TestHarness.Configuration
{
	public interface IRouteConfigurator
	{
		IRouteConfigurator Named(string name);
		IRouteConfigurator WithTemplate(string template);
		IRouteConfigurator AddParameter(RouteConfigurationParameter parameter);
		IRouteConfigurator AddParameters(params RouteConfigurationParameter[] parameter);
	}
}
