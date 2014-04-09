using System;

namespace WebApi.TestHarness.Configuration
{
	[Serializable]
	public class RouteConfigurationParameter
	{
		public string Name { get; set; }
		public bool IsOptional { get; set; }
		public object Value { get; set; }

		public static RouteConfigurationParameter Create(string name, object value = null)
		{
			return new RouteConfigurationParameter
			{
				Name = name,
				IsOptional = (value == null),
				Value = value
			};
		}
	}
}
