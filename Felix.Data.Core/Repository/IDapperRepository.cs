using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Felix.Data.Core.Repository
{
    public interface IDapperRepository
    {
        int ExecuteSql(string sql, object param = null);
        Task<int> ExecuteSqlAsync(string sql, object param = null);
        int ExecuteStoreProcedure(string sql, object param = null);
        Task<int> ExecuteStoreProcedureAsync(string sql, object param = null);
        IDataReader ExecuteSqlReader(string sql, object param = null);
        Task<IDataReader> ExecuteSqlReaderAsync(string sql, object param = null);
        IDataReader ExecuteReaderStoreProcedure(string sql, object param = null);
        Task<IDataReader> ExecuteReaderStoreProcedureAsync(string sql, object param = null);
        T ExecuteScalar<T>(string sql, object param = null);
        Task<T> ExecuteScalarAsync<T>(string sql, object param = null);
        GridReader QueryMultiple(string sql, object param = null);
        Task<GridReader> QueryMultipleAsync(string sql, object param = null);
        IEnumerable<T> QuerySql<T>(string sql, object param = null);
        Task<IEnumerable<T>> QuerySqlAsync<T>(string sql, object param = null);
        IEnumerable<T> QueryStoreProcedure<T>(string sql, object param = null);
        Task<IEnumerable<T>> QueryStoreProcedureAsync<T>(string sql, object param = null);
        T QueryFirst<T>(string sql, object param = null);
        Task<T> QueryFirstAsync<T>(string sql, object param = null);
        T QueryFirstOrDefault<T>(string sql, object param = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null);
        T QuerySingle<T>(string sql, object param = null);
        Task<T> QuerySingleAsync<T>(string sql, object param = null);
        T QuerySingleOrDefault<T>(string sql, object param = null);
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null);
    }
}
