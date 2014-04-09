using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using NUnit.Framework;

using WebApi.TestHarness.Configuration;

// ReSharper disable InconsistentNaming

namespace WebApi.TestHarness.Tests
{
	[TestFixture]
	public class WebServiceHostFactoryTests
	{
		private const string _hostedServiceUrl = "http://localhost:12345/api/TestApi";

		private static HttpStatusCode HttpGet<T>(out T result)
		{
			result = default (T);

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
		public void CreateFor_ValidApiControllerAndHostConfiguration_SuccessfullyHostsService()
		{
			IEnumerable<TestObject> hostedResult;

			using (WebServiceHostFactory.CreateFor<TestApiController>(configure =>
				configure.WithRoutes(routes =>
					routes
						.WithBase("http://localhost:12345")
						.AddRoute("DefaultRoute", "api/{controller}/{id}", RouteConfigurationParameter.Create("id")))))
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

			// using the actual type here defeats our purpose; it would cause that type
			// to be loaded into this AppDomain. hence, the magic string
			Assert.That(assemblyNamesLoadedAfter, Has.No.Member("Microsoft.Owin.Hosting"));
		}

		[Test]
		public void CreateFor_ConfiguredWebServiceHostWithMockDependencyResolver_DependantAssembliesNotLoadedAfterDispose()
		{
			IEnumerable<TestObject> hostedResult;

			using (WebServiceHostFactory.CreateFor<TestApiController>(configure =>
				configure.WithRoutes(routes =>
					routes
						.WithBase("http://localhost:12345")
						.AddRoute(route => 
							route
								.Named("DefaultRoute")
								.WithTemplate("api/{controller}/{id}")
								.AddParameter(RouteConfigurationParameter.Create("id"))))
					.UsingDependencyResolver<MockDependencyResolver>()))
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

			// using the actual type here defeats our purpose; it would cause that type
			// to be loaded into this AppDomain. hence, the magic string
			Assert.That(assemblyNamesLoadedAfter, Has.No.Member("Microsoft.Owin.Hosting"));
		}
	}
}
