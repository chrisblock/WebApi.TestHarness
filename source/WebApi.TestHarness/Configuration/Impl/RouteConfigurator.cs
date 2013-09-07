using System.Collections.Generic;

namespace WebApi.TestHarness.Configuration.Impl
{
	public class RouteConfigurator : IRouteConfigurator
	{
		private string _name;
		private string _template;
		private readonly ICollection<RouteConfigurationParameter> _parameters;

		public RouteConfigurator()
		{
			_parameters = new List<RouteConfigurationParameter>();
		}

		public IRouteConfigurator Named(string name)
		{
			_name = name;

			return this;
		}

		public IRouteConfigurator WithTemplate(string template)
		{
			_template = template;

			return this;
		}

		public IRouteConfigurator AddParameter(RouteConfigurationParameter parameter)
		{
			_parameters.Add(parameter);

			return this;
		}

		public IRouteConfigurator AddParameters(params RouteConfigurationParameter[] parameters)
		{
			foreach (var parameter in parameters)
			{
				_parameters.Add(parameter);
			}

			return this;
		}

		public RouteConfiguration BuildConfiguration()
		{
			return new RouteConfiguration
			       {
			       	Name = _name,
			       	Template = _template,
			       	DefaultParameters = _parameters
			       };
		}
	}
}
