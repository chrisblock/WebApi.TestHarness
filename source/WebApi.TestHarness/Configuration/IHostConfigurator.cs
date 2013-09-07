using System;
using System.Web.Http.Dependencies;

namespace WebApi.TestHarness.Configuration
{
	public interface IHostConfigurator
	{
		IHostConfigurator UsingDependencyResolver<T>() where T : IDependencyResolver;

		IHostConfigurator WithRoutes(RouteConfigurationTable routeConfigurationTable);
		IHostConfigurator WithRoutes(Func<IRouteTableConfigurator, IRouteTableConfigurator> configure);
	}
}
