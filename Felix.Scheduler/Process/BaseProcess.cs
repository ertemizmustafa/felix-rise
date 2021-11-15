using Dapper;
using Felix.Common.Extensions;
using Felix.Common.Helpers;
using Felix.Data.Core.Model;
using Felix.Scheduler.Entities;
using Felix.Scheduler.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Process
{
    public abstract class BaseProcess
    {
        protected readonly string _connectionString;
        protected readonly string _transactionId;
        protected readonly string _jobId;
        protected readonly string _jobName;
        protected readonly ILogger _logger;

        public BaseProcess(string connectionString, string transactionId, string jobId, string jobName)
        {
            _connectionString = connectionString;
            _transactionId = transactionId;
            _jobId = jobId;
            _jobName = jobName;
            _logger = LogHelper.GetApplicationLogger<BaseProcess>();
        }

        public abstract Task<dynamic> FetchAsync<T>(ResourceProcessModel model);
        public abstract Task<dynamic> SendAsync(TargetProcessModel model);

        protected virtual object ConvertDictionaryToDapperDynamicParameter(IDictionary<string, object> dictionary)
        {
            if (dictionary == null)
                return dictionary;

            return new DynamicParameters(dictionary);
        }

        protected virtual IEnumerable<IDictionary<string, object>> TransformTargetDataToDictionary(IEnumerable<object> list, IEnumerable<ScheduleTargetParameterEntity> mappings)
        {
            var isDictionary = list is List<IDictionary<string, object>> || list.OfType<object>().Any(x => x is IDictionary<string, object>);

            return isDictionary ? list.Select(item => mappings.ToDictionary(m => m.TargetName, m => !string.IsNullOrEmpty(m.TargetValue) ? m.TargetValue : (item as IDictionary<string, object>)[m.MappingName]))
                : list.Select(item => mappings.ToDictionary(m => m.TargetName, m => !string.IsNullOrEmpty(m.TargetValue) ? m.TargetValue : item.GetType().GetProperty(m.MappingName).GetValue(item)));
        }


        protected virtual List<BaseColumn> TransformToBaseColumns(IEnumerable<ScheduleTargetParameterEntity> mappings, List<ScheduleParameterTypeEntity> scheduleParameterTypes)
        {
            var baseColumn = mappings.ContainItem() ? mappings.Select(x => new BaseColumn { ColumnName = x.TargetName, ColumnType = Type.GetType(scheduleParameterTypes.FirstOrDefault(i => i.Id == x.ParameterTypeId).Description), IsNullable = x.IsNullable, IsIdentity = x.IsIdentity, ParameterName = x.MappingName, ParameterValue = string.IsNullOrEmpty(x.MappingName) ? x.TargetValue : null }).ToList() : new List<BaseColumn>();
            return baseColumn;
        }

        protected virtual string ApplyCommandParametersToSoapCommand(string command, IDictionary<string, object> commandParameters)
        {
            if (!string.IsNullOrEmpty(command) && commandParameters.ContainItem())
            {
                commandParameters.ToList().ForEach(x =>
                    {
                        command.Replace(x.Key, x.Value.ToString());
                    });
            }

            return command;
        }
    }
}
