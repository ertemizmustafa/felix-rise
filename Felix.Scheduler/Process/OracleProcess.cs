using Felix.Data.Core.UnitOfWork;
using Felix.Scheduler.Enums;
using Felix.Scheduler.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Process
{
    public class OracleProcess : BaseProcess
    {
        public OracleProcess(string connectionString, string transactionId, string jobId, string jobName) : base(connectionString, transactionId, jobId, jobName)
        {

        }
        public override async Task<dynamic> FetchAsync<T>(ResourceProcessModel model)
        {
            using var uow = new UnitOfWork<OracleConnection>(_connectionString);
            dynamic result = model.ProcessType switch
            {
                (int)ProcessTypeEnum.Query => result = await uow.DapperRepository().QuerySqlAsync<T>(model.Command, ConvertDictionaryToDapperDynamicParameter(model.CommandParameters)),
                (int)ProcessTypeEnum.Table => result = await uow.DapperRepository().QuerySqlAsync<T>($"SELECT * FROM {model.Command}", ConvertDictionaryToDapperDynamicParameter(model.CommandParameters)),
                (int)ProcessTypeEnum.Procedure => result = await uow.DapperRepository().ExecuteReaderStoreProcedureAsync(model.Command, ConvertDictionaryToDapperDynamicParameter(model.CommandParameters)),
                _ => throw new Exception("Invalid process"),
            };

            return result;
        }

        public override async Task<dynamic> SendAsync(TargetProcessModel model)
        {
            using var uow = new UnitOfWork<OracleConnection>(_connectionString);
            dynamic result = model.CommandProcessTypeId switch
            {
                (int)ProcessTypeEnum.Query => result = await uow.DapperRepository().ExecuteSqlAsync(model.Command, ConvertDictionaryToDapperDynamicParameter(model.CommandParameters)),
                (int)ProcessTypeEnum.Table => result = uow.AdoRepository().BulkInsertToOracle(model.DataList.ToList(), new IntegrationTable(model.Command, TransformToBaseColumns(model.TargetParameterMappings, model.ScheduleParameterTypes), model.DeletePreviousData)),
                (int)ProcessTypeEnum.Procedure => result = await uow.DapperRepository().ExecuteStoreProcedureAsync(model.Command, ConvertDictionaryToDapperDynamicParameter(model.CommandParameters)),
                _ => throw new Exception("Invalid process"),
            };

            return result;
        }
    }
}
