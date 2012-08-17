using System;
using System.Web.Http.SelfHost;

using WebApi.TestHarness.Configuration;

namespace WebApi.TestHarness.Hosting.Impl
{
	internal class WebServiceHost : MarshalByRefObject, IWebServiceHost
	{
		private readonly HttpSelfHostServer _server;

		private bool _isStarted = false;

		public WebServiceHost(RouteConfigurationTable routeTable)
		{
			var config = new HttpSelfHostConfiguration(routeTable.BaseUri);

			config.Routes.Configure(routeTable);

			_server = new HttpSelfHostServer(config);

			Start();
		}

		private void Start()
		{
			if (_isStarted == false)
			{
				var openTask = _server.OpenAsync();

				openTask.Wait();

				if (openTask.IsFaulted || (openTask.Exception != null))
				{
					throw new ApplicationException("Unable to open web service host.");
				}

				_isStarted = true;
			}
		}

		private void Stop()
		{
			if (_isStarted)
			{
				var closeTask = _server.CloseAsync();

				closeTask.Wait();

				if (closeTask.IsFaulted || (closeTask.Exception != null))
				{
					throw new ApplicationException("Unable to close web service host.");
				}
			}
		}

		public void Dispose()
		{
			Stop();
		}
	}
}
