using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

using WebApi.TestHarness.Impl;

namespace WebApi.TestHarness
{
	public static class WebServiceHostFactory
	{
		public static IWebServiceHost CreateFor<T>(string url)
			where T : ApiController
		{
			return CreateFor<T>(new Uri(url));
		}

		public static IWebServiceHost CreateFor<T>(Uri uri)
			where T : ApiController
		{
			var type = typeof(T);

			var current = AppDomain.CurrentDomain;

			var domain = AppDomain.CreateDomain(String.Format("TestWebServiceDomainFor_{0}", type.Name), current.Evidence, current.BaseDirectory, current.BaseDirectory, false);

			domain.LoadAssemblyContainingType<HttpSelfHostServer>();
			domain.LoadAssemblyContainingType<WebServiceHostProxy>();
			domain.LoadAssemblyContainingType<T>();

			var result = new WebServiceHostProxy(domain, uri);

			return result;
		}
	}
}
