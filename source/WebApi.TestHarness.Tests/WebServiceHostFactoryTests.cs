// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using NUnit.Framework;

using WebApi.TestHarness.Configuration;

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

				if (getTask.IsFaulted)
				{
					var exception = getTask.Exception ?? new Exception(String.Format("Could not GET from url '{0}'.", _hostedServiceUrl));

					throw exception;
				}

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
			IEnumerable<TestObject> hostedResult;

			var routeTable = new RouteConfigurationTable("http://localhost:12345", new[]
			{
				new RouteCounfiguration
				{
					Name = "DefaultRoute",
					Template = "api/{controller}/{id}",
					DefaultParameters = new List<RouteConfigurationParameter>
					{
						RouteConfigurationParameter.Create("id")
					}
				}
			});

			using (var host = WebServiceHostFactory.CreateFor<TestApiController>(routeTable))
			{
				HttpGet(out hostedResult);
			}

			Assert.That(hostedResult, Is.Not.Null);
			Assert.That(hostedResult, Is.Not.Empty);

			IEnumerable<TestObject> unHostedResult;
			Assert.That(() => HttpGet(out unHostedResult), Throws.Exception);

			var assemblyNamesLoadedAfter = AppDomain.CurrentDomain.GetAssemblies()
				.Select(x => x.GetName().Name)
				.ToList();

			Assert.That(assemblyNamesLoadedAfter, Has.No.Member("System.Web.Http.SelfHost"));
		}
	}
}
