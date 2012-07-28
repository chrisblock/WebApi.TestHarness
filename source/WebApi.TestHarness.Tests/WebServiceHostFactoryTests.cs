// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using NUnit.Framework;

namespace WebApi.TestHarness.Tests
{
	[TestFixture]
	public class WebServiceHostFactoryTests
	{
		private const string _hostedServiceUrl = "http://localhost:12345/api/TestApi";

		private static HttpStatusCode HttpGet<T>(out T result)
		{
			result = default(T);

			HttpStatusCode status;

			using (var httpClient = new HttpClient())
			{
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var getTask = httpClient.GetAsync(_hostedServiceUrl);

				getTask.Wait();

				var httpResponse = getTask.Result;

				status = httpResponse.StatusCode;

				if (httpResponse.IsSuccessStatusCode)
				{
					var contentTask = httpResponse.Content.ReadAsAsync<T>();

					result = contentTask.Result;
				}
			}

			return status;
		}

		[Test]
		public void CreateFor_ValidApiControllerAndRouteConfiguration_SuccessfullyHostsService()
		{
			IEnumerable<string> hostedResult;

			using (var host = WebServiceHostFactory.CreateFor<TestApiController>("http://localhost:12345"))
			{
				var routeTable = new RouteConfigurationTable(new[]
				{
					new RouteCounfiguration
					{
						Name = "DefaultRouteWithId",
						Template = "api/{controller}/{id}",
						DefaultParameters = new List<RouteConfigurationParameter>
						{
							RouteConfigurationParameter.Create("id")
						}
					}
				});

				host.Start(routeTable);

				HttpGet(out hostedResult);
			}

			Assert.That(hostedResult, Is.Not.Null);
			Assert.That(hostedResult, Is.Not.Empty);

			IEnumerable<string> unHostedResult;
			Assert.That(() => HttpGet(out unHostedResult), Throws.Exception);
		}
	}
}
