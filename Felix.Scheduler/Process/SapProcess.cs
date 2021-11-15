using Felix.Data.Core.UnitOfWork;
using Felix.Scheduler.Enums;
using Felix.Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Process
{
    public class SapProcess : BaseProcess
    {
        public SapProcess(string connectionString, string transactionId, string jobId, string jobName) : base(connectionString, transactionId, jobId, jobName)
        {

        }
        public override async Task<dynamic> FetchAsync<T>(ResourceProcessModel model)
        {
            dynamic result = null;
            await Task.Run(() =>
            {
                //    using var sapConnection = new SapConnection(_connectionString))
                //    var sapResult = sapConnection.SapRepository().Execute(model.Command, model.CommandParameters);
                //    result = model.IsSingleValue ? sapResult.GetSingleValueAsEnumerableDictionary(model.CommandOutputName) : sapResult.GetEnumarableDictionary(model.CommandOutputName);
            });

            return result;
        }

        public override async Task<dynamic> SendAsync(TargetProcessModel model)
        {
            dynamic result = null;
            await Task.Run(() =>
            {
                //    using var sapConnection = new SapConnection(_connectionString))
                var insertValues = TransformTargetDataToDictionary(model.DataList, model.TargetParameterMappings);
                //result = sapConnection.SapRepository().InsertTable(model.CommandParameters,model.ServiceOutput,model.CommandParameters,insertValues).GetEnumarableDictionary(model.CommandOutputName);
                
            });

            return result;
        }
    }
}
