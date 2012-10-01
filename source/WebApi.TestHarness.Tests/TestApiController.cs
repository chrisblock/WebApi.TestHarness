using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApi.TestHarness.Tests
{
	public class TestApiController : ApiController
	{
		public IEnumerable<TestObject> Get()
		{
			var result = Enumerable.Range(0, 5).Select(x => new TestObject
			{
				Id = x,
				Name = String.Format("{0}", x)
			});

			return result;
		}
	}
}
