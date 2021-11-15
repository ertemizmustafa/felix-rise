using Felix.Data.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Model
{
    public class IntegrationTable : BaseTable
    {
        public IntegrationTable(string tableName, List<BaseColumn> baseColumns, bool deletePreviousData = false)
        {
            TableName = tableName;
            DeletePreviousData = deletePreviousData;
            Columns = baseColumns;
            //Add Default Colmn
            //Columns.Add(new BaseColumn {  ColumnName = "CreateDate" vsvs})
        }
    }
}
