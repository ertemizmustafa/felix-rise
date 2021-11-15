using Felix.Data.Core.Model;
using Felix.Data.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xunit;

namespace Felix.Data.Core.Test.Test
{
    public class AdoRepositoryTest
    {
        [Fact]
        public void InsertBulkToSqlServer_Value_NonTransaction()
        {
            using var uow = new UnitOfWork<SqlConnection>("ConnectionStringss");

            var integrationTable = new IntegrationTable("Table1", new List<BaseColumn> {
            new BaseColumn {ColumnName = "TargetValue1", ColumnType = System.Type.GetType("System.Int32"), ParameterName = "Value1"},
            new BaseColumn {ColumnName = "TargetValue2", ColumnType = System.Type.GetType("System.String"), ParameterName = "Value2"},
            new BaseColumn {ColumnName = "TargetValue3", ColumnType = System.Type.GetType("System.String"), ParameterName = "Value3"}});

            var list = new List<dynamic>
            {
                new FromSomething { Value1 = 1, Value2 = "Test Value 2", Value3 = "Test Value3"},
                new FromSomething { Value1 = 2, Value2 = "Test Value 2", Value3 = "Test Value3"},
            };

            var result = uow.AdoRepository().BulkInsertToSqlServer(list, integrationTable);

            Assert.True(result > 0);
        }
    }

    public class IntegrationTable : BaseTable
    {
        public IntegrationTable(string tableName, List<BaseColumn> baseColumns)
        {
            DeletePreviousData = true;
            TableName = tableName;
            Columns = baseColumns;
        }
    }


    public class FromSomething
    {
        public int Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
    }
}
