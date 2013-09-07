using System;
using System.Collections.Generic;
using System.Linq;

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
	}
}
