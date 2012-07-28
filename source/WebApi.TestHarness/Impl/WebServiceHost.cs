using System;
using System.Web.Http.SelfHost;

namespace WebApi.TestHarness.Impl
{
	internal class WebServiceHost : MarshalByRefObject, IWebServiceHost
	{
		private readonly Uri _baseUri;
		private HttpSelfHostServer _server;

		public WebServiceHost(Uri baseUri)
		{
			_baseUri = baseUri;
		}

		public void Start(RouteConfigurationTable routeConfigurationTable)
		{
			if (_server == null)
			{
				var config = new HttpSelfHostConfiguration(_baseUri);

				routeConfigurationTable.Configure(config.Routes);

				_server = new HttpSelfHostServer(config);

				var openTask = _server.OpenAsync();

				openTask.Wait();

				if (openTask.IsFaulted || (openTask.Exception != null))
				{
					throw new ApplicationException("Unable to open web service host.");
				}
			}
		}

		public void Dispose()
		{
			if (_server != null)
			{
				var closeTask = _server.CloseAsync();

				closeTask.Wait();

				if (closeTask.IsFaulted || (closeTask.Exception != null))
				{
					throw new ApplicationException("Unable to close web service host.");
				}
			}
		}
	}
}
