using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;

namespace WebApi.TestHarness.Configuration
{
	[Serializable]
	public class RouteConfigurationTable
	{
		public Uri BaseUri { get; private set; }

		private readonly IList<RouteConfiguration> _configurations;
		public IEnumerable<RouteConfiguration> Configurations { get { return _configurations; } }

		public RouteConfigurationTable(string baseUrl) : this(new Uri(baseUrl))
		{
		}

		public RouteConfigurationTable(Uri baseUri)
		{
			BaseUri = baseUri;
			_configurations = new List<RouteConfiguration>();
		}

		public RouteConfigurationTable(string baseUrl, params RouteConfiguration[] routes) : this(new Uri(baseUrl), routes.AsEnumerable())
		{
		}

		public RouteConfigurationTable(Uri baseUri, params RouteConfiguration[] routes) : this(baseUri, routes.AsEnumerable())
		{
		}

		public RouteConfigurationTable(string baseUrl, IEnumerable<RouteConfiguration> routes) : this(new Uri(baseUrl), routes)
		{
		}

		public RouteConfigurationTable(Uri baseUri, IEnumerable<RouteConfiguration> routes) : this(baseUri)
		{
			foreach (var route in routes)
			{
				Add(route);
			}
		}

		public void Add(RouteConfiguration entry)
		{
			_configurations.Add(entry);
		}

		public void Add(string name, string template, IEnumerable<RouteConfigurationParameter> defaultParameters = null)
		{
			var entry = new RouteConfiguration
			{
				Name = name,
				Template = template,
				DefaultParameters = defaultParameters
			};

			Add(entry);
		}

		public void Configure(HttpRouteCollection routeCollection)
		{
			foreach (var routeCounfiguration in Configurations)
			{
				var defaultParameters = routeCounfiguration.DefaultParameters ?? Enumerable.Empty<RouteConfigurationParameter>();

				var routeValueDictionary = BuildHttpRouteValueDictionary(defaultParameters);

				var route = new HttpRoute(routeCounfiguration.Template, routeValueDictionary);

				routeCollection.Add(routeCounfiguration.Name, route);
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
