using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.TestHarness.Configuration
{
	[Serializable]
	public class RouteConfigurationTable
	{
		public Uri BaseUri { get; private set; }

		private readonly IList<RouteCounfiguration> _configurations;
		public IEnumerable<RouteCounfiguration> Configurations { get { return _configurations; } }

		public RouteConfigurationTable(string baseUrl) : this(new Uri(baseUrl))
		{
		}

		public RouteConfigurationTable(Uri baseUri)
		{
			BaseUri = baseUri;
			_configurations = new List<RouteCounfiguration>();
		}

		public RouteConfigurationTable(string baseUrl, params RouteCounfiguration[] routes) : this(new Uri(baseUrl), routes.AsEnumerable())
		{
		}

		public RouteConfigurationTable(Uri baseUri, params RouteCounfiguration[] routes) : this(baseUri, routes.AsEnumerable())
		{
		}

		public RouteConfigurationTable(string baseUrl, IEnumerable<RouteCounfiguration> routes) : this(new Uri(baseUrl), routes)
		{
		}

		public RouteConfigurationTable(Uri baseUri, IEnumerable<RouteCounfiguration> routes) : this(baseUri)
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
	}
}
