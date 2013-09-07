using System;
using System.Collections.Generic;

namespace WebApi.TestHarness.Configuration.Impl
{
	public class RouteTableConfigurator : IRouteTableConfigurator
	{
		private Uri _baseUri;
		private readonly ICollection<RouteConfiguration> _routes;

		public RouteTableConfigurator()
		{
			_routes = new List<RouteConfiguration>();
		}

		public IRouteTableConfigurator WithBase(string baseUrl)
		{
			var uri = new Uri(baseUrl);

			return WithBase(uri);
		}

		public IRouteTableConfigurator WithBase(Uri baseUri)
		{
			_baseUri = baseUri;

			return this;
		}

		public IRouteTableConfigurator AddRoute(RouteConfiguration routeConfiguration)
		{
			_routes.Add(routeConfiguration);

			return this;
		}

		public IRouteTableConfigurator AddRoute(string name, string template, params RouteConfigurationParameter[] defaultParameters)
		{
			var routeConfiguration = new RouteConfiguration
			{
				Name = name,
				Template = template,
				DefaultParameters = defaultParameters
			};

			return AddRoute(routeConfiguration);
		}

		public IRouteTableConfigurator AddRoute(Func<IRouteConfigurator, IRouteConfigurator> configure)
		{
			var configurator = new RouteConfigurator();

			configurator = (RouteConfigurator) configure(configurator);

			return AddRoute(configurator.BuildConfiguration());
		}

		public RouteConfigurationTable BuildConfiguration()
		{
			return new RouteConfigurationTable(_baseUri, _routes);
		}
	}
}
