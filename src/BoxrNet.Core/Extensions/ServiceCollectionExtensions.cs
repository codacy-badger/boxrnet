using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace BoxrNet.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Get a required service of type <see cref="TService"/>
        /// <exception cref="InvalidOperationException"></exception>
        /// </summary>
        /// <typeparam name="TService">The type of settings object to get</typeparam>
        /// <param name="services">ServiceCollection instance<</param>
        /// <returns>Returns a instance of <see cref="TService"/></returns>
        public static TService GetRequiredService<TService>(this IServiceCollection services) =>
            services.BuildServiceProvider().GetRequiredService<TService>();

        /// <summary>
        /// Get service of type <see cref="TService"/>
        /// </summary>
        /// <typeparam name="TService">The type of settings object to get</typeparam>
        /// <param name="services">ServiceCollection instance<</param>
        /// <returns>Returns a instance of <see cref="TService"/></returns>
        public static TService GetService<TService>(this IServiceCollection services) =>
            services.BuildServiceProvider().GetService<TService>();

        /// <summary>
        /// Register a configuration of <see cref="TSettings"/> type at dependency injection container.
        /// <para>
        /// Uses Options Pattern <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-2.2"/>
        /// </para>
        /// </summary>
        /// <typeparam name="TSettings">The type of settings object to add</typeparam>
        /// <param name="services">ServiceCollection instance</param>
        /// <param name="sectionName">Settings section name</param>
        /// <param name="implementedInterfaceName">Settings type interface name, if exists</param>
        /// <returns></returns>
        public static IServiceCollection AddSettings<TSettings>(
            this IServiceCollection services,
            string sectionName,
            string implementedInterfaceName = default)
            where TSettings : class, new()
        {
            var configuration = services.GetRequiredService<IConfiguration>();
            services.Configure<TSettings>(configuration.GetSection(sectionName));

            return services.TryAddSettings<TSettings>(implementedInterfaceName);
        }

        private static IServiceCollection TryAddSettings<TSettings>(
            this IServiceCollection services,
            string implementedInterfaceName)
            where TSettings : class, new()
        {
            var settings = services.GetRequiredService<IOptions<TSettings>>();
            var @interface = ExtractInterface<TSettings>(implementedInterfaceName);

            if (settings?.Value is null)
            {
                throw new InvalidOperationException("Invalid configuration settings");
            }

            if (@interface != null)
            {
                services.TryAddSingleton(@interface, (provider) => settings.Value);
                return services;
            }

            services.TryAddSingleton(settings.Value);

            return services;
        }

        private static Type ExtractInterface<TSettings>(string implementedInterfaceName)
            where TSettings : class, new()
        {
            var settingsType = typeof(TSettings);

            return string.IsNullOrEmpty(implementedInterfaceName)
                ? settingsType.GetInterfaces().FirstOrDefault()
                : settingsType.GetInterface(implementedInterfaceName);
        }
    }
}
