using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;

namespace WebApi.TestHarness
{
	[Serializable]
	public class RouteConfigurationTable
	{
		private readonly IList<RouteCounfiguration> _configurations;
		public IEnumerable<RouteCounfiguration> Configurations { get { return _configurations; } }

		public RouteConfigurationTable()
		{
			_configurations = new List<RouteCounfiguration>();
		}

		public RouteConfigurationTable(params RouteCounfiguration[] routes) : this(routes.AsEnumerable())
		{
		}

		public RouteConfigurationTable(IEnumerable<RouteCounfiguration> routes) : this()
		{
			foreach (var route in routes)
			{
				Add(route);
			}
		}

		public void Add(RouteCounfiguration entry)
		{
			_configurations.Add(entry);
		}

		public void Add(string name, string template, IEnumerable<RouteConfigurationParameter> defaultParameters = null)
		{
			var entry = new RouteCounfiguration
			{
				Name = name,
				Template = template,
				DefaultParameters = defaultParameters
			};

			Add(entry);
		}

		public void Configure(HttpRouteCollection httpRouteCollection)
		{
			foreach (var routeTableEntry in Configurations)
			{
				var defaultParameters = routeTableEntry.DefaultParameters ?? Enumerable.Empty<RouteConfigurationParameter>();

				var routeValueDictionary = BuildHttpRouteValueDictionary(defaultParameters);

				var route = new HttpRoute(routeTableEntry.Template, routeValueDictionary);

				httpRouteCollection.Add(routeTableEntry.Name, route);
			}
		}

		private static HttpRouteValueDictionary BuildHttpRouteValueDictionary(IEnumerable<RouteConfigurationParameter> defaultParameters)
		{
			var result = new HttpRouteValueDictionary();

			foreach (var defaultParameter in defaultParameters)
			{
				var value = defaultParameter.IsOptional
					? RouteParameter.Optional
					: defaultParameter.Value;

				result.Add(defaultParameter.Name, value);
			}

			return result;
		}
	}
}
