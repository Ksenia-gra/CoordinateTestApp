using CoordinateTestApp.Repository;
using CoordinateTestApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateTestApp.MVVM.IoC
{
	public static class IoC
	{
		private static IServiceProvider provider;
		/// <summary>
		/// Dependency registration
		/// </summary>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="System.Reflection.ReflectionTypeLoadException"></exception>
		/// <exception cref="System.Reflection.TargetInvocationException"></exception>"
		public static void Init()
		{
			ServiceCollection services = new ServiceCollection();
			IEnumerable<Type> assemblyTypes = typeof(IoC).Assembly.GetTypes().Where(t => t.IsClass);

			foreach (Type type in assemblyTypes)
			{
				Type[] interfaces = type.GetInterfaces();

				if (interfaces.Contains(typeof(ISingleton)))
				{
					services.AddSingleton(type);
				}
				else if (interfaces.Contains(typeof(ITransient)))
				{
					services.AddTransient(type);
				}
			}
			services.AddSingleton<IPointRepository, CachePointRepository>();
			services.TryAdd(ServiceDescriptor.Singleton(typeof(ICacheCreator<>), typeof(FileCacheCreator<>)));

			provider = services.BuildServiceProvider();
			foreach (Type singltoneType in services.Where(x => x.Lifetime == ServiceLifetime.Singleton && !x.ServiceType.IsGenericType).Select(x => x.ServiceType))
			{
				_ = provider.GetRequiredService(singltoneType);
			}
		}

		/// <summary>
		/// Get app services
		/// </summary>
		/// <returns>Type of service</returns>
		/// <exception cref="InvalidOperationException"></exception>
		public static T GetRequiredService<T>()
		{
			return provider.GetRequiredService<T>();
		}

		public interface ISingleton
		{

		}
		public interface ITransient
		{

		}
	}
}
