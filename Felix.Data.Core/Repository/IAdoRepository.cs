using Felix.Data.Core.Model;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Felix.Data.Core.Repository
{
    public interface IAdoRepository
    {
        DataTable ExecuteStoreProcedureToDataTable(string commandText, Dictionary<string, object> parameters, int? commandTimeout = null);
        DataTable ExecuteDataTable(string commandText, Dictionary<string, object> parameters = null, int? commandTimeout = null);
        int ExecuteText(string commandText, Dictionary<string, object> parameters = null, int? commandTimeout = null);
        int ExecuteStoreProcedure(string commandText, Dictionary<string, object> parameters = null, int? commandTimeout = null);
        void BulkInsertToSqlServer(string tableName, DataTable dataTable);
        int BulkInsertToSqlServer<TTable>(IList insertDataList, TTable table) where TTable : BaseTable;
        int BulkInsertToOracle<TTable>(IList insertDataList, TTable table) where TTable : BaseTable;
    }
}
