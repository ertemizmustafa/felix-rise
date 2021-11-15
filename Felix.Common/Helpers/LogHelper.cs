using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Felix.Common.Helpers
{
    public static class LogHelper
    {
        public const string Template = "{TransactionId}{Message}{CreatedBy}{ActionStartTime}{ActionEndTime}{ActionCallerIp}";

        public static ILogger GetApplicationLogger<T>()
        {
            var sinkOpt = new MSSqlServerSinkOptions()
            {
                TableName = "LogTable"
            };

            return new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.MSSqlServer("Connectionstring", sinkOpt, columnOptions: GetColumnOptions()).CreateLogger();
        }

        public static ColumnOptions GetColumnOptions()
        {
            var columnOptions = new ColumnOptions();

            columnOptions.Store.Remove(StandardColumn.MessageTemplate);
            columnOptions.Store.Remove(StandardColumn.Message);
            columnOptions.Store.Remove(StandardColumn.LogEvent);
            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.TimeStamp.ColumnName = "ActionEndTime";

            columnOptions.AdditionalColumns = new List<SqlColumn>
            {
                new SqlColumn {DataType = System.Data.SqlDbType.NVarChar, ColumnName = "Message" },
                new SqlColumn {DataType = System.Data.SqlDbType.NVarChar, ColumnName = "TransactionId" },
                new SqlColumn {DataType = System.Data.SqlDbType.NVarChar, ColumnName = "CreatedBy" },
                new SqlColumn {DataType = System.Data.SqlDbType.DateTime, ColumnName = "ActionStartTime" },
                new SqlColumn {DataType = System.Data.SqlDbType.NVarChar, ColumnName = "ActionCallerIp" }
            };

            return columnOptions;
        }
    }
}
