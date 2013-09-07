using System;

using WebApi.TestHarness.Configuration;

namespace WebApi.TestHarness.Hosting.Impl
{
	internal class WebServiceHostProxy : IWebServiceHost
	{
		private readonly AppDomain _webServiceHostDomain;
		private readonly IWebServiceHost _webServiceHost;

		public WebServiceHostProxy(AppDomain webServiceHostDomain, HostConfiguration routeTable)
		{
			_webServiceHostDomain = webServiceHostDomain;
			_webServiceHost = _webServiceHostDomain.CreateInstanceAndUnwrap<WebServiceHost>(routeTable);
		}

		public void Dispose()
		{
			_webServiceHost.Dispose();

			AppDomain.Unload(_webServiceHostDomain);
		}
	}
}
