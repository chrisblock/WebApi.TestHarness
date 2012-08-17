using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebApi.TestHarness
{
	internal static class MemberInfoExtensions
	{
		public static IEnumerable<T> GetCustomAttributes<T>(this MemberInfo memberInfo, bool inherit = false)
			where T : Attribute
		{
			return memberInfo.GetCustomAttributes(typeof (T), inherit).Cast<T>();
		}
	}
}
