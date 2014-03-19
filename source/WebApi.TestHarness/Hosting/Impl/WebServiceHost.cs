using System;
using System.Web.Http.Dependencies;
using System.Web.Http.SelfHost;

using WebApi.TestHarness.Configuration;

namespace WebApi.TestHarness.Hosting.Impl
{
	internal class WebServiceHost : MarshalByRefObject, IWebServiceHost
	{
		private readonly HttpSelfHostServer _server;

		public WebServiceHost(HostConfiguration hostConfiguration)
		{
			var routeTable = hostConfiguration.RouteTable;

			var config = new HttpSelfHostConfiguration(routeTable.BaseUri);

			if (hostConfiguration.DependencyResolverType != null)
			{
				var dependencyResolver = Activator.CreateInstance(hostConfiguration.DependencyResolverType, config.DependencyResolver);

				config.DependencyResolver = (IDependencyResolver) dependencyResolver;
			}

			config.Routes.Configure(routeTable);

			_server = new HttpSelfHostServer(config);

			var openTask = _server.OpenAsync();

			openTask.Wait();

			if (openTask.IsFaulted || (openTask.Exception != null))
			{
				throw new ApplicationException("Unable to open web service host.");
			}
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

				var closeTask = _server.CloseAsync();

				closeTask.Wait();

				if (closeTask.IsFaulted || (closeTask.Exception != null))
				{
					throw new ApplicationException("Unable to close web service host.");
				}

				_server.Dispose();
			}

			// dispose native resources
		}
	}
}
