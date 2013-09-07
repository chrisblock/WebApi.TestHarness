using System;
using System.Web.Http.Dependencies;

namespace WebApi.TestHarness.Configuration.Impl
{
	public class HostConfigurator : IHostConfigurator
	{
		private Type _dependencyResolverType;
		private RouteConfigurationTable _routeConfigurationTable;

		public IHostConfigurator UsingDependencyResolver<T>()
			where T : IDependencyResolver
		{
			_dependencyResolverType = typeof (T);

			return this;
		}

		public IHostConfigurator WithRoutes(RouteConfigurationTable routeConfigurationTable)
		{
			_routeConfigurationTable = routeConfigurationTable;

			return this;
		}

		public IHostConfigurator WithRoutes(Func<IRouteTableConfigurator, IRouteTableConfigurator> configure)
		{
			var routeConfigurator = new RouteTableConfigurator();

			routeConfigurator = (RouteTableConfigurator) configure(routeConfigurator);

			_routeConfigurationTable = routeConfigurator.BuildConfiguration();

			return this;
		}

		public HostConfiguration BuildConfiguration()
		{
			var result = new HostConfiguration
			{
				DependencyResolverType = _dependencyResolverType,
				RouteTable = _routeConfigurationTable
			};

			return result;
		}
	}
}
