using System;

namespace WebApi.TestHarness.Impl
{
	internal class WebServiceHostProxy : IWebServiceHost
	{
		private readonly AppDomain _webServiceHostDomain;
		private readonly IWebServiceHost _webServiceHost;

		public WebServiceHostProxy(AppDomain webServiceHostDomain, Uri uri)
		{
			_webServiceHostDomain = webServiceHostDomain;
			_webServiceHost = _webServiceHostDomain.CreateInstanceAndUnwrap<WebServiceHost>(uri);
		}

		public void Start(RouteConfigurationTable routeConfigurationTable)
		{
			_webServiceHost.Start(routeConfigurationTable);
		}

		public void Dispose()
		{
			_webServiceHost.Dispose();

			AppDomain.Unload(_webServiceHostDomain);
		}
	}
}
