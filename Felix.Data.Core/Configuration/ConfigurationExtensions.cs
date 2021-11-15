using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Felix.Data.Core.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddDatabaseConnection(this IServiceCollection services, string connectionString)
        {
            CacheExtensions.ConnectionString.Set("ConnectionString", connectionString);
            return services;
        }
    }
}
