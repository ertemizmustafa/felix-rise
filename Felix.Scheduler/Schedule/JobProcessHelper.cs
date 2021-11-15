using Felix.Common.Extensions;
using Felix.Common.Helpers;
using Felix.Data.Core.UnitOfWork;
using Felix.Scheduler.Entities;
using Felix.Scheduler.Enums;
using Felix.Scheduler.Model;
using Felix.Scheduler.Process;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Schedule
{
    public class JobProcessHelper
    {
        private readonly string _processNameSpace;
        private readonly string _transactionId;
        private readonly string _jobId;
        private readonly string _jobName;
        private IUnitOfWork _uow;
        private static ILogger _processLogger;

        public JobProcessHelper(IUnitOfWork uow, string transactionId, string jobId, string jobName)
        {
            _uow = uow;
            _transactionId = transactionId ?? Guid.NewGuid().ToString();
            _jobId = jobId;
            _jobName = jobName;
            _processNameSpace = typeof(BaseProcess).Namespace;
            _processLogger = LogHelper.GetApplicationLogger<JobProcessHelper>();
        }

        private BaseProcess GetBaseProcess(int configurationId)
        {

            var configuration = _uow.FastCrudRepository<ScheduleConfigurationEntity>().Get(new ScheduleConfigurationEntity { Id = configurationId });
            var configurationName = configuration.ConfigurationName;
            var configurationType = _uow.FastCrudRepository<ScheduleConfigurationTypeEntity>().Get(new ScheduleConfigurationTypeEntity { Id = configuration.ConfigurationTypeId });
            var process = (BaseProcess)ActivatorExtensions.CreateInstance(Type.GetType($"{_processNameSpace}.{configurationType.Description}"), new object[] { configurationName, _transactionId, _jobId, _jobName });
            return process;
        }

        private ResourceProcessModel GetResourceProcessModel(int resourceId)
        {
            var resource = _uow.FastCrudRepository<ScheduleResourceEntity>().Get(new ScheduleResourceEntity { Id = resourceId });
            var resourceParameters = _uow.FastCrudRepository<ScheduleResourceParameterEntity>().Find(x => x.Where($"{nameof(ScheduleResourceParameterEntity.ResourceId):C} = @ParamResourceId")
                                                                                                            .WithParameters(new { ParamResourceId = resourceId })).ToDictionary(x => x.Name, x => x.RunTimeParameterId.HasValue ? ConvertRunTimeParameter(x.RunTimeParameterId.Value, x.Value, x.RunTimeParameterFormat) : x.Value) as IDictionary<string, object>;

            var result = new ResourceProcessModel
            {
                ResourceId = resource.Id,
                Description = resource.Description,
                Service = resource.ResourceService,
                ServiceOutput = resource.ResourceServiceOutput,
                Command = resource.ResourceCommand,
                ProcessType = resource.ResourceProcessTypeId,
                CommandOutputName = resource.ResourceCommandOutput,
                CommandParameters = resourceParameters,
                IsSingleValue = resource.IsSingleValue
            };
            return result;
        }

        private TargetProcessModel GetTargetProcessModel(int targetId, dynamic insertValues)
        {
            var paramTypes = _uow.FastCrudRepository<ScheduleParameterTypeEntity>().Find();
            var target = _uow.FastCrudRepository<ScheduleTargetEntity>().Get(new ScheduleTargetEntity { Id = targetId });
            var targetParameters = _uow.FastCrudRepository<ScheduleTargetParameterEntity>().Find(x => x.Where($"{nameof(ScheduleTargetParameterEntity.TargetId):C} = @ParamTargetId").WithParameters(new { ParamTargetId = targetId }));

            var result = new TargetProcessModel
            {
                TargetId = target.Id,
                Description = target.Description,
                DataList = insertValues != null ? (insertValues is IEnumerable<dynamic> ? insertValues : new List<dynamic> { insertValues }) : null,
                DeletePreviousData = target.DeletePreviousData,
                Service = target.TargetService,
                ServiceOutput = target.TargetServiceOutput,
                Command = target.TargetCommand,
                TargetParameterMappings = targetParameters.Where(x => !x.IsCommandParameter).ToList(),
                CommandParameters = targetParameters.Where(x => x.IsCommandParameter).ToDictionary(x => x.TargetName, x => x.RunTimeParameterId.HasValue ? ConvertRunTimeParameter(x.RunTimeParameterId.Value, x.TargetValue, x.RunTimeTargetValueFormat) : x.TargetValue) as IDictionary<string, object>,
                CommandProcessTypeId = target.TargetCommandProcessTypeId,
                CommandOutputName = target.TargetCommandOutput,
                ScheduleParameterTypes = paramTypes.ToList()
            };
            return result;
        }

        public dynamic ProcessTarget(int targetId, dynamic insertValues)
        {
            //loglama burada
            var target = _uow.FastCrudRepository<ScheduleTargetEntity>().Get(new ScheduleTargetEntity { Id = targetId });
            var targetProcess = GetBaseProcess(target.ConfigurationId);
            var targetProcessModel = GetTargetProcessModel(target.Id, insertValues);
            var targetSendResponse = targetProcess.SendAsync(targetProcessModel).Result;
            return targetSendResponse;
        }


        public dynamic ProcessResource(int resourceId)
        {
            //loglama burada
            var resource = _uow.FastCrudRepository<ScheduleResourceEntity>().Get(new ScheduleResourceEntity { Id = resourceId });
            var resourceProcess = GetBaseProcess(resource.ConfigurationId);
            var resourceProcessModel = GetResourceProcessModel(resource.Id);
            var response = resourceProcess.FetchAsync<dynamic>(resourceProcessModel).Result;
            return response;
        }


        private object ConvertRunTimeParameter(int runTimeParameterType, string value, string format)
        {
            object result = null;
            switch (runTimeParameterType)
            {
                case (int)ProcessRunTimeParameterEnum.CurrentDate:
                    result = DateTime.Now.Date;
                    break;
                case (int)ProcessRunTimeParameterEnum.FirstDayOfMonth:
                    result = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
                    break;
                case (int)ProcessRunTimeParameterEnum.LastDayOfMonth:
                    result = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date.AddMonths(1).AddDays(-1).Date;
                    break;
                case (int)ProcessRunTimeParameterEnum.CurrentDateValueCombination:
                    result = DateTime.Now.AddDays(int.Parse(value));
                    break;
            }

            if (result != null && !string.IsNullOrEmpty(format) && result.GetType() == typeof(DateTime))
            {
                result = ((DateTime)result).ToString(format, CultureInfo.InvariantCulture);
            }
            return result;
        }

    }
}
