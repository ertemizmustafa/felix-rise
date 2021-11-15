using Felix.Data.Core.UnitOfWork;
using Felix.Schedule.Core.Infrastructure;
using Felix.Scheduler.Entities;
using Hangfire;
using Hangfire.Server;
using Hangfire.Storage;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Schedule
{
    public class JobScheduler
    {

        private readonly IBackgroundProcessingClient _backgroundProcessingClient;

        public JobScheduler(IBackgroundProcessingClient backgroundProcessingClient) => _backgroundProcessingClient = backgroundProcessingClient;

        public void AddScheduledJobs()
        {
            using var uow = new UnitOfWork<SqlConnection>();

            var jobs = uow.FastCrudRepository<ScheduleJobEntity>().Find();

            jobs.Where(x => !x.ParentId.HasValue).ToList().ForEach(x =>
            {
                var reccuringJobId = $"{x.Id.ToString()} - {x.Description}";
                if (x.IsActive)
                {
                    _backgroundProcessingClient.AddReccuringJob(() => new JobProcess().StartProcessItem(null, x.Id, x.Description), reccuringJobId, x.Cron);
                }
                else
                {
                    _backgroundProcessingClient.RemoveReccuringJob(reccuringJobId);
                }
            });

            //5dk bir degisiklik dinle
            _backgroundProcessingClient.AddReccuringJob(() => new JobProcess().AddListener(), "Job Listener", "*/5 * * * * ");
        }

        public void ProcessChainedJobs(PerformContext context, List<ScheduleJobEntity> childs, string jobId, int scheduleItemId)
        {
            childs.Where(x => x.ParentId == scheduleItemId).ToList().ForEach(item =>
            {
                var returnJobId = _backgroundProcessingClient.AddChainedJob(jobId, () => new JobProcess().ProcessItem(context, new Model.ScheduleProcessItemModel { Id = item.Id, Description = item.Description, ParentId = item.ParentId }), item.CanContinueIfFails ? Hangfire.JobContinuationOptions.OnAnyFinishedState : Hangfire.JobContinuationOptions.OnlyOnSucceededState);
                ProcessChainedJobs(context, childs, returnJobId, item.Id);
            });
        }


        public void JobListenerProcess(int jobId, string description, string cron, string reccuringJobId, bool isActive)
        {
            if (!string.IsNullOrEmpty(reccuringJobId) && !CheckIsJobRunning(reccuringJobId))
            {
                if (isActive)
                    _backgroundProcessingClient.AddReccuringJob(() => new JobProcess().StartProcessItem(null, jobId, description), reccuringJobId, cron);
                else
                    _backgroundProcessingClient.RemoveReccuringJob(reccuringJobId);

            }
        }

        public bool CheckIsJobRunning(string reccuringJobId)
        {
            var isJobRunning = false;

            using var connection = JobStorage.Current.GetConnection();
            StorageConnectionExtensions.GetRecurringJobs(connection).Where(item => item.Id == reccuringJobId).ToList().ForEach(item =>
            {
                if (item.LastJobState == "Processing")
                    isJobRunning = true;
            });


            return isJobRunning;
        }

    }
}
