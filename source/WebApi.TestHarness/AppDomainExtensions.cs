using System;
using System.Globalization;
using System.Reflection;

namespace WebApi.TestHarness
{
	internal static class AppDomainExtensions
	{
		public static T CreateInstanceAndUnwrap<T>(this AppDomain appDomain, params object[] constructorParameters)
			where T : MarshalByRefObject
		{
			var type = typeof (T);

			var assembly = type.Assembly;

			var typeName = type.FullName;

			return (T) appDomain.CreateInstanceAndUnwrap(assembly.FullName, typeName, false, BindingFlags.Instance | BindingFlags.Public, null, constructorParameters, CultureInfo.InvariantCulture, new object[0]);
		}

		public static void LoadAssemblyContainingType<T>(this AppDomain appDomain)
		{
			var type = typeof (T);

			appDomain.LoadAssemblyContainingType(type);
		}

		public static void LoadAssemblyContainingType(this AppDomain appDomain, Type type)
		{
			var assembly = type.Assembly;
			var assemblyName = assembly.GetName();

			appDomain.Load(assemblyName);
		}
	}
}
