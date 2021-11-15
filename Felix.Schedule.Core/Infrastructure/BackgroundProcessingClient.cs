using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Felix.Schedule.Core.Infrastructure
{
    public class BackgroundProcessingClient : IBackgroundProcessingClient
    {

        //private readonly IServiceProvider _serviceProvider;
        public BackgroundProcessingClient()
        {

        }

        //public BackgroundProcessingClient(IServiceProvider serviceProvider)
        //{
        //    _serviceProvider = serviceProvider;
        //}

        public string AddChainedJob(string parentId, Expression<Action> action, JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState) => BackgroundJob.ContinueJobWith(parentId, action, options);

        public void AddReccuringJob(Expression<Action> action, string reccuringJobId, string cron) => RecurringJob.AddOrUpdate(reccuringJobId, action, cron, timeZone: TimeZoneInfo.Local);

        public string Enqueue(Expression<Action> action) => BackgroundJob.Enqueue(action);

        public void RemoveReccuringJob(string reccuringJobId) => RecurringJob.RemoveIfExists(reccuringJobId);
    }
}
