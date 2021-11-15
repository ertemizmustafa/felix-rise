using Felix.Data.Core.Common;
using Felix.Data.Core.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Felix.Data.Core.Repository
{
    public class AdoRepository : IAdoRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        internal AdoRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public int BulkInsertToOracle<TTable>(IList insertDataList, TTable table) where TTable : BaseTable
        {
            var dt = new DataTable();
            if (table.DeletePreviousData)
            {
                var deleteSql = table.GenerateDeleteScript(true);
                var deleteCommand = (OracleCommand)CreateCommand(deleteSql, CommandType.Text);
                deleteCommand.ExecuteNonQuery();
            }

            var insertCommandText = table.GenerateInsertScript(true);
            var command = (OracleCommand)CreateCommand(insertCommandText, CommandType.Text, commandTimeout: 9999);
            command.BindByName = true;

            int index = 1;
            int processedCount = 0;
            var isDictionaryList = insertDataList is List<IDictionary<string, object>> || insertDataList.OfType<object>().Any(x => x is IDictionary<string, object>);

            //Split into pieces
            var slicedList = Extensions.Split(new List<object>(insertDataList.OfType<object>()), Constants.MaxDataInsertCount);

            slicedList.ForEach(containerList =>
            {
                index = 1;
                containerList.ForEach(item =>
                {
                    table.Columns.Where(col => (!col.IsAutoIncremental || (col.IsIdentity = false))).ToList().ForEach(column =>
                    {
                        if (!string.IsNullOrEmpty(column.ParameterName))
                        {
                            if (isDictionaryList)
                            {
                                column.DataValues.Add((item as IDictionary<string, object>)[column.ParameterName]);
                            }
                            else
                            {
                                column.DataValues.Add(column.ParameterValue ?? item.GetType().GetProperty(column.ParameterName).GetValue(item));
                            }
                        }
                    });


                    //Maks insert sayısına ulastıgında
                    if (index >= Constants.MaxDataInsertCount || (containerList.Count < Constants.MaxDataInsertCount && index >= containerList.Count))
                    {
                        command.ArrayBindCount = index;
                        command.Parameters.Clear();

                        table.Columns.Where(col => (!col.IsAutoIncremental || (col.IsIdentity = false)) && col.DataValues != null).ToList().ForEach(dataCol =>
                        {
                            OracleParameter oracleParameter = new OracleParameter
                            {
                                ParameterName = string.Concat(":", dataCol.ParameterName),
                                OracleDbType = Constants.OracleTypeMap[dataCol.ColumnType],
                                Direction = ParameterDirection.Input
                            };

                            oracleParameter.Value = dataCol.DataValues.Take(index).Select(data => Convert.ChangeType(data.ToString(), dataCol.ColumnType)).ToArray();
                            command.Parameters.Add(oracleParameter);
                        });
                        command.ExecuteNonQuery();
                        processedCount += index;
                    }
                    index++;
                });
            });

            return processedCount;
        }

        public void BulkInsertToSqlServer(string tableName, DataTable dataTable)
        {
            var bulkCopy = new SqlBulkCopy((SqlConnection)_connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null)
            {
                DestinationTableName = tableName
            };

            bulkCopy.WriteToServer(dataTable);
        }

        public int BulkInsertToSqlServer<TTable>(IList insertDataList, TTable table) where TTable : BaseTable
        {
            var dt = new DataTable();
            if (table.DeletePreviousData)
            {
                var deleteSql = table.GenerateDeleteScript();
                var command = (SqlCommand)CreateCommand(deleteSql, CommandType.Text);
                command.ExecuteNonQuery();
            }

            var isDictionaryList = insertDataList is List<IDictionary<string, object>> || insertDataList.OfType<object>().Any(x => x is IDictionary<string, object>);
            dt.Columns.AddRange(table.Columns.Select(x => new DataColumn { AllowDBNull = x.IsNullable, ColumnName = x.ColumnName, DataType = x.ColumnType }).ToArray());
            foreach (var item in insertDataList)
            {
                var row = dt.NewRow();
                foreach (var column in table.Columns)
                {
                    if (isDictionaryList)
                    {
                        row[column.ColumnName] = column.ParameterValue ?? (item as IDictionary<string, object>)[column.ParameterName] ?? DBNull.Value;
                    }
                    else
                    {
                        row[column.ColumnName] = column.ParameterValue ?? item.GetType().GetProperty(column.ParameterName).GetValue(item) ?? DBNull.Value;
                    }
                }
                dt.Rows.Add(row);
            }

            var bulkCopy = new SqlBulkCopy((SqlConnection)_connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null)
            {
                DestinationTableName = table.TableName
            };

            foreach (var column in table.Columns)
            {
                bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping { DestinationColumn = column.ColumnName, SourceColumn = column.ColumnName });
            }

            bulkCopy.WriteToServer(dt);

            return insertDataList.Count;
        }

        public DataTable ExecuteDataTable(string commandText, Dictionary<string, object> parameters = null, int? commandTimeout = null)
        {
            var dt = new DataTable();
            var command = CreateCommand(commandText, CommandType.Text, parameters, commandTimeout);
            command.Transaction = _transaction;
            using var reader = command.ExecuteReader();
            dt.Load(reader);
            return dt;

        }

        public int ExecuteStoreProcedure(string commandText, Dictionary<string, object> parameters = null, int? commandTimeout = null)
        {
            var command = CreateCommand(commandText, CommandType.StoredProcedure, parameters, commandTimeout);
            command.Transaction = _transaction;
            return command.ExecuteNonQuery();
        }

        public DataTable ExecuteStoreProcedureToDataTable(string commandText, Dictionary<string, object> parameters, int? commandTimeout = null)
        {
            var dt = new DataTable();
            var command = CreateCommand(commandText, CommandType.StoredProcedure, parameters, commandTimeout);
            command.Transaction = _transaction;
            using var reader = command.ExecuteReader();
            dt.Load(reader);
            return dt;
        }

        public int ExecuteText(string commandText, Dictionary<string, object> parameters = null, int? commandTimeout = null)
        {
            var command = CreateCommand(commandText, CommandType.Text, parameters, commandTimeout);
            command.Transaction = _transaction;
            return command.ExecuteNonQuery();
        }

        private IDbCommand CreateCommand(string commandText, CommandType commandType, Dictionary<string, object> parameters = null, int? commandTimeout = null)
        {
            var command = _connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            command.CommandTimeout = commandTimeout ?? 60;

            if (parameters != null && parameters.Any())
            {
                parameters.Select(x => CreateSqlParameter(x.Key, x.Value)).ToList().ForEach(item =>
                {
                    command.Parameters.Add(item);
                });
            }

            return command;
        }

        private SqlParameter CreateSqlParameter(string parameterName, object parameterValue)
        {
            return new SqlParameter($"@{parameterName}", parameterValue);
        }
    }
}
