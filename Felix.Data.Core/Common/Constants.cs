using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Felix.Data.Core.Common
{
    public class Constants
    {
        public const int MaxDataInsertCount = 5000;

        public static readonly Dictionary<Type, DbType> SqlServerTypeMap = new Dictionary<Type, DbType>()
        {
            {typeof(byte), DbType.Byte },
            {typeof(int), DbType.Int32 },
            {typeof(long), DbType.Int64 },
            {typeof(double), DbType.Double },
            {typeof(decimal), DbType.Decimal },
            {typeof(bool), DbType.Boolean },
            {typeof(string), DbType.String },
            {typeof(DateTime), DbType.DateTime },
            {typeof(DateTimeOffset), DbType.DateTimeOffset },
            {typeof(byte[]), DbType.Binary },
            {typeof(byte?), DbType.Byte },
            {typeof(int?), DbType.Byte },
            {typeof(long?), DbType.Int64 },
            {typeof(double?), DbType.Double },
            {typeof(decimal?), DbType.Decimal },
            {typeof(bool?), DbType.Boolean },
            {typeof(DateTime?), DbType.DateTime }
        };

        public static readonly Dictionary<Type, OracleDbType> OracleTypeMap = new Dictionary<Type, OracleDbType>()
        {
            {typeof(byte), OracleDbType.Byte },
            {typeof(int), OracleDbType.Int32 },
            {typeof(long), OracleDbType.Int64 },
            {typeof(double), OracleDbType.Double },
            {typeof(decimal), OracleDbType.Decimal },
            {typeof(bool), OracleDbType.Boolean },
            {typeof(string), OracleDbType.NVarchar2 },
            {typeof(DateTime), OracleDbType.Date },
            {typeof(DateTimeOffset), OracleDbType.Date },
            {typeof(byte[]), OracleDbType.Blob },
            {typeof(byte?), OracleDbType.Byte },
            {typeof(int?), OracleDbType.Byte },
            {typeof(long?), OracleDbType.Int64 },
            {typeof(double?), OracleDbType.Double },
            {typeof(decimal?), OracleDbType.Decimal },
            {typeof(bool?), OracleDbType.Boolean },
            {typeof(DateTime?), OracleDbType.Date }
        };
    }
}
