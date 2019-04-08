using Microsoft.Extensions.Configuration;
using System;

namespace BoxrNet.Tests.Helpers
{
    public static class ConfigurationHelper
    {
        public static IConfigurationRoot GetConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static IConfiguration GetConfiguration()
        {
            return GetConfigurationRoot();
        }
    }
}