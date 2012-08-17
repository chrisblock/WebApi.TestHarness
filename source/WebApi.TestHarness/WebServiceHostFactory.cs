using System;
using System.Web.Http;

using WebApi.TestHarness.Configuration;
using WebApi.TestHarness.Hosting;
using WebApi.TestHarness.Hosting.Impl;

namespace WebApi.TestHarness
{
	public static class WebServiceHostFactory
	{
		public static IWebServiceHost CreateFor<T>(RouteConfigurationTable routeTable)
			where T : ApiController
		{
			var type = typeof(T);

			var current = AppDomain.CurrentDomain;

			var domain = AppDomain.CreateDomain(String.Format("TestWebServiceDomainFor_{0}", type.Name), current.Evidence, current.BaseDirectory, current.BaseDirectory, false);

			domain.LoadAssemblyContainingType<WebServiceHostProxy>();
			domain.LoadAssemblyContainingType<T>();

			var result = new WebServiceHostProxy(domain, routeTable);

			return result;
		}
	}
}
