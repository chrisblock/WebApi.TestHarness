using System;
using System.Web.Http;
using System.Web.Http.Dependencies;

using Microsoft.Owin.Hosting;

using Owin;

using WebApi.TestHarness.Configuration;

namespace WebApi.TestHarness.Hosting.Impl
{
	internal class WebServiceHost : MarshalByRefObject, IWebServiceHost
	{
		private readonly IDisposable _server;

		public WebServiceHost(HostConfiguration hostConfiguration)
		{
			var routeTable = hostConfiguration.RouteTable;

			var configuration = new HttpConfiguration();

			if (hostConfiguration.DependencyResolverType != null)
			{
				var dependencyResolver = Activator.CreateInstance(hostConfiguration.DependencyResolverType, configuration.DependencyResolver);

				configuration.DependencyResolver = (IDependencyResolver) dependencyResolver;
			}

			routeTable.Configure(configuration.Routes);

			_server = WebApp.Start(routeTable.BaseUri.ToString(), builder =>
			{
				builder.UseWebApi(configuration);
			});
		}

		~WebServiceHost()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				// dispose managed resources

				_server.Dispose();
			}

			// dispose native resources
		}
	}
}
