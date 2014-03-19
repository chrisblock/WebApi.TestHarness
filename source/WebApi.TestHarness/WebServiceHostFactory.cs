using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dependencies;

using WebApi.TestHarness.Configuration;
using WebApi.TestHarness.Configuration.Impl;
using WebApi.TestHarness.Hosting;
using WebApi.TestHarness.Hosting.Impl;

namespace WebApi.TestHarness
{
	public static class WebServiceHostFactory
	{
		private static void EnsureValidConfiguration(this HostConfiguration hostConfiguration)
		{
			var dependencyResolverType = hostConfiguration.DependencyResolverType;

			if (dependencyResolverType != null)
			{
				if (typeof (IDependencyResolver).IsAssignableFrom(dependencyResolverType) == false)
				{
					throw new ArgumentException(String.Format("Type '{0}' does not implement the interface '{1}'.", dependencyResolverType, typeof (IDependencyResolver)));
				}
				
				if (dependencyResolverType.GetConstructors().Any(HasSingleParameterOfTypeIDependencyResolver) == false)
				{
					throw new ArgumentException(String.Format("Type '{0}' does not have a parameterless constructor.", dependencyResolverType));
				}
			}
		}

		private static bool HasSingleParameterOfTypeIDependencyResolver(ConstructorInfo constructorInfo)
		{
			var parameters = constructorInfo.GetParameters();

			return (parameters.Length == 1) && (parameters.Single().ParameterType == typeof (IDependencyResolver));
		}

		public static IWebServiceHost CreateFor<T>(HostConfiguration hostConfiguration)
			where T : ApiController
		{
			hostConfiguration.EnsureValidConfiguration();

			var type = typeof (T);

			var current = AppDomain.CurrentDomain;

			var domain = AppDomain.CreateDomain(String.Format("TestWebServiceDomainFor_{0}", type.Name), current.Evidence, current.BaseDirectory, current.RelativeSearchPath, false);

			domain.LoadAssemblyContainingType<WebServiceHostProxy>();
			domain.LoadAssemblyContainingType<T>();

			var result = new WebServiceHostProxy(domain, hostConfiguration);

			return result;
		}

		public static IWebServiceHost CreateFor<T>(Func<IHostConfigurator, IHostConfigurator> configure)
			where T : ApiController
		{
			var configurator = new HostConfigurator();

			configurator = (HostConfigurator) configure(configurator);

			var configuration = configurator.BuildConfiguration();

			return CreateFor<T>(configuration);
		}
	}
}
