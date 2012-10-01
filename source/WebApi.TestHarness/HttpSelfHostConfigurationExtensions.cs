using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;

using WebApi.TestHarness.Configuration;

namespace WebApi.TestHarness
{
	public static class HttpSelfHostConfigurationExtensions
	{
		public static void Configure(this HttpRouteCollection httpRouteCollection, RouteConfigurationTable routeTable)
		{
			foreach (var routeCounfiguration in routeTable.Configurations)
			{
				var defaultParameters = routeCounfiguration.DefaultParameters ?? Enumerable.Empty<RouteConfigurationParameter>();

				var routeValueDictionary = BuildHttpRouteValueDictionary(defaultParameters);

				var route = new HttpRoute(routeCounfiguration.Template, routeValueDictionary);

				httpRouteCollection.Add(routeCounfiguration.Name, route);
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
