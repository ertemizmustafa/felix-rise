using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Felix.Schedule.Core.Infrastructure
{
    public interface IBackgroundProcessingClient
    {

        string Enqueue(Expression<Action> action);
        string AddChainedJob(string parentId, Expression<Action> action, JobContinuationOptions options = JobContinuationOptions.OnlyOnSucceededState);
        void RemoveReccuringJob(string reccuringJobId);
        void AddReccuringJob(Expression<Action> action, string reccuringJobId, string cron);

    }
}
