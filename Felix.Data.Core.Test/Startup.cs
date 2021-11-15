using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Felix.Data.Core.Configuration;

namespace Felix.Data.Core.Test
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder) => hostBuilder
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: false);
            });

        public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        {
            services.AddDatabaseConnection(context.Configuration.GetConnectionString("ConnectionString"));
        }
    }
}
