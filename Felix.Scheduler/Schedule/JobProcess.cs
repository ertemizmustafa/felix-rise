using Felix.Common.Helpers;
using Felix.Data.Core.UnitOfWork;
using Felix.Schedule.Core.Infrastructure;
using Felix.Scheduler.Entities;
using Felix.Scheduler.Model;
using Hangfire.Server;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Schedule
{
    public class JobProcess
    {
        private static ILogger _applicationLogger;
        private static string _transactionId;
        private static DateTime _actionStartTime;

        public JobProcess()
        {
            _applicationLogger = LogHelper.GetApplicationLogger<JobProcess>();
            _transactionId = Guid.NewGuid().ToString();
            _actionStartTime = DateTime.Now;
        }


        public void StartProcessItem(PerformContext context, int jobId, string description)
        {
            using var uow = new UnitOfWork<SqlConnection>();

            var connectedJobs = uow.FastCrudRepository<ScheduleJobEntity>().FindAsync(x => x
                        .Where($"{nameof(ScheduleJobEntity.IsActive):C} = @ParamIsActive")
                        .WithParameters(new { ParamIsActive = true })).Result.Where(x => x.ParentId.HasValue).ToList();

            var backgroundProcessJobId = context.BackgroundJob.Id;

            ProcessItem(context, new ScheduleProcessItemModel { Description = description, Id = jobId, ParentId = 0 });

            //Child varsa tüm işler birbirine baglı calısır.
            new JobScheduler(new BackgroundProcessingClient()).ProcessChainedJobs(context, connectedJobs, backgroundProcessJobId, jobId);


        }

        public void AddListener()
        {
            using var uow = new UnitOfWork<SqlConnection>();
            var jobs = uow.FastCrudRepository<ScheduleJobEntity>().FindAsync().Result;

            jobs.Where(x => !x.ParentId.HasValue).ToList().ForEach(x =>
            {
                var reccuringJobId = $"{x.Id.ToString()} - {x.Description}";
                new JobScheduler(new BackgroundProcessingClient()).JobListenerProcess(x.Id, x.Description, x.Cron, reccuringJobId, x.IsActive);
            });
        }

        public bool ProcessItem(PerformContext context, ScheduleProcessItemModel model)
        {
            var resultMail = "";
            try
            {
                using var uow = new UnitOfWork<SqlConnection>();

                var job = uow.FastCrudRepository<ScheduleJobEntity>().Get(new ScheduleJobEntity { Id = model.Id });

                if (job == null)
                    throw new Exception("Not found");


                resultMail = job.ResultMail;
                var jobProcessHelper = new JobProcessHelper(uow, _transactionId, job.Id.ToString(), job.Description);

                if (job.RetryInterval.HasValue)
                {
                    var retryAttempt = context.GetJobParameter<int>("RetryCount");
                    if (retryAttempt > job.RetryInterval.Value)
                    {
                        context.SetJobParameter("RetryCount", 999 + 1);
                        //Mail ve loglama
                    }
                    throw new Exception("Exceed");
                }

                var targetResource = uow.FastCrudRepository<ScheduleTargetEntity>().Get(new ScheduleTargetEntity { Id = job.TargetId });

                if (targetResource == null)
                    throw new Exception("Not found");


                dynamic resourceResponse = null;
                if (job.ResourceId.HasValue)
                {
                    resourceResponse = jobProcessHelper.ProcessResource(job.ResourceId.Value);

                    if (resourceResponse == null && job.SkipTargetIfResourceDataEmpty)
                    {
                        return true;
                    }
                    else
                    {
                        throw new Exception("No data fetch");
                    }
                }

                //Before
                if (targetResource.BeforeTargetId.HasValue)
                {
                    var beforeTargetResponse = jobProcessHelper.ProcessTarget(targetResource.BeforeTargetId.Value, null);
                }

                //target
                var targetResponse = jobProcessHelper.ProcessTarget(targetResource.Id, resourceResponse);
                if (targetResource.UseAsResourceForAfterProcess && targetResource == null)
                {
                    throw new Exception("Target Response null");
                }

                //After
                if (targetResource.AfterTargetId.HasValue)
                {
                    var afterTargetResponse = jobProcessHelper.ProcessTarget(targetResource.AfterTargetId.Value, targetResource.UseAsResourceForAfterProcess ? targetResponse : null);
                }

                if (!string.IsNullOrEmpty(resultMail))
                {
                    //mail at
                }

                return model.Id > 0;

            }
            catch (Exception ex)
            {
                //log mail
                throw ex;
            }
        }
    }
}
