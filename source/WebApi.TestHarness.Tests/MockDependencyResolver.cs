using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace WebApi.TestHarness.Tests
{
	public class MockDependencyResolver : IDependencyResolver
	{
		private readonly IDependencyResolver _oldDependencyResolver;
		private readonly ICollection<string> _arguments;

		public MockDependencyResolver(IDependencyResolver oldDependencyResolver)
		{
			_oldDependencyResolver = oldDependencyResolver;
			_arguments = new List<string>();
		}

		public object GetService(Type serviceType)
		{
			var argument = String.Format("{0}", serviceType);

			_arguments.Add(argument);

			object result = _oldDependencyResolver.GetService(serviceType);

			return result;
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			var argument = String.Format("{0}", serviceType);

			_arguments.Add(argument);

			return _oldDependencyResolver.GetServices(serviceType);
		}

		public IDependencyScope BeginScope()
		{
			return this;
		}

		public void Dispose()
		{
		}
	}
}
