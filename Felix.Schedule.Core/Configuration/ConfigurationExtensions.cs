using Felix.Schedule.Core.Infrastructure;
using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Felix.Schedule.Core.Configuration
{
    public static class ConfigurationExtensions
    {

        public static IServiceCollection AddBackgroundJobServer(this IServiceCollection collection, BackgroundJobServerSettings settings)
        {
            var options = new SqlServerStorageOptions
            {
                QueuePollInterval = TimeSpan.FromMinutes(1),
                SchemaName = settings.DbSchemaName
            };

            GlobalConfiguration.Configuration.UseSqlServerStorage(settings.ConnectionString, options);
            collection.AddHangfire(x => x
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseSqlServerStorage(settings.ConnectionString, options));

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Fail, DelaysInSeconds = new int[] { 60 } });
            GlobalJobFilters.Filters.Add(new ContinuationsSupportAttribute(true));
            GlobalJobFilters.Filters.Add(new ProlongExpirationTimeAttribute());


            collection
                .AddTransient<IBackgroundProcessingClient, BackgroundProcessingClient>()
                .AddSingleton(settings);

            return collection;
        }

        public static IApplicationBuilder AddDashBoard(this IApplicationBuilder app)
        {
            GlobalConfiguration.Configuration.UseActivator(new BackgroundProcessActivator(app?.ApplicationServices));
            var setttings = app.ApplicationServices.GetService<BackgroundJobServerSettings>();

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                WorkerCount = 10,
                SchedulePollingInterval = TimeSpan.FromSeconds(1),
                ServerCheckInterval = TimeSpan.FromSeconds(1),
                HeartbeatInterval = TimeSpan.FromSeconds(1)
            })
                .UseHangfireDashboard(setttings.DashboardEnpoint, new DashboardOptions
                {
                    Authorization = new[] { new ScheduleDashboardAuthenticationFilter() },
                    DashboardTitle = "My Title",
                    IgnoreAntiforgeryToken = true
                });

            return app;
        }
    }


    public class ScheduleDashboardAuthenticationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var htppContext = context.GetHttpContext();
            //Is in role de kullan ekstra
            //htppContext?.User ?.IsInRole("ADMIN")
           var isAuth = htppContext?.User?.Identity?.IsAuthenticated ?? false;
            return true;
        }
    }


    public class ProlongExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
    {
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = TimeSpan.FromDays(7);
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = TimeSpan.FromDays(7);
        }
    }
}
