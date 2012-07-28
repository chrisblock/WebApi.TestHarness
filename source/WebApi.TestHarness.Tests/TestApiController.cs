using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApi.TestHarness.Tests
{
	public class TestApiController : ApiController
	{
		public IEnumerable<string> Get()
		{
			return Enumerable.Range(0, 5).Select(x => String.Format("{0}", x));
		}
	}
}
