using System;

namespace WebApi.TestHarness.Impl
{
	internal class WebServiceHostProxy : IWebServiceHost
	{
		private readonly AppDomain _webServiceHostDomain;
		private readonly IWebServiceHost _webServiceHost;

		public WebServiceHostProxy(AppDomain webServiceHostDomain, RouteConfigurationTable routeTable)
		{
			_webServiceHostDomain = webServiceHostDomain;
			_webServiceHost = _webServiceHostDomain.CreateInstanceAndUnwrap<WebServiceHost>(routeTable);
		}

		public void Start()
		{
			_webServiceHost.Start();
		}

		public void Stop()
		{
			_webServiceHost.Stop();
		}

		public void Dispose()
		{
			_webServiceHost.Dispose();

			AppDomain.Unload(_webServiceHostDomain);
		}
	}
}
