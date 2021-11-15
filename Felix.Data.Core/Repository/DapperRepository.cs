using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Felix.Data.Core.Repository
{
    public class DapperRepository : IDapperRepository
    {
        private readonly IDbConnection _connecion;
        private readonly IDbTransaction _transaction;

        internal DapperRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connecion = connection;
            _transaction = transaction;
        }

        public IDataReader ExecuteReaderStoreProcedure(string sql, object param = null)
        {
            return _connecion.ExecuteReader(sql, param, transaction: _transaction, commandType: CommandType.StoredProcedure);
        }

        public Task<IDataReader> ExecuteReaderStoreProcedureAsync(string sql, object param = null)
        {
            return _connecion.ExecuteReaderAsync(sql, param, transaction: _transaction, commandType: CommandType.StoredProcedure);
        }

        public T ExecuteScalar<T>(string sql, object param = null)
        {
            return _connecion.ExecuteScalar<T>(sql, param, transaction: _transaction);
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object param = null)
        {
            return _connecion.ExecuteScalarAsync<T>(sql, param, transaction: _transaction);
        }

        public int ExecuteSql(string sql, object param = null)
        {
            return _connecion.Execute(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public Task<int> ExecuteSqlAsync(string sql, object param = null)
        {
            return _connecion.ExecuteAsync(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public IDataReader ExecuteSqlReader(string sql, object param = null)
        {
            return _connecion.ExecuteReader(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public Task<IDataReader> ExecuteSqlReaderAsync(string sql, object param = null)
        {
            return _connecion.ExecuteReaderAsync(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public int ExecuteStoreProcedure(string sql, object param = null)
        {
            return _connecion.Execute(sql, param, transaction: _transaction, commandType: CommandType.StoredProcedure);
        }

        public Task<int> ExecuteStoreProcedureAsync(string sql, object param = null)
        {
            return _connecion.ExecuteAsync(sql, param, transaction: _transaction, commandType: CommandType.StoredProcedure);
        }

        public T QueryFirst<T>(string sql, object param = null)
        {
            return _connecion.QueryFirst<T>(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public Task<T> QueryFirstAsync<T>(string sql, object param = null)
        {
            return _connecion.QueryFirstAsync<T>(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public T QueryFirstOrDefault<T>(string sql, object param = null)
        {
            return _connecion.QueryFirstOrDefault<T>(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null)
        {
            return _connecion.QueryFirstOrDefaultAsync<T>(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public SqlMapper.GridReader QueryMultiple(string sql, object param = null)
        {
            return _connecion.QueryMultiple(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null)
        {
            return _connecion.QueryMultipleAsync(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public T QuerySingle<T>(string sql, object param = null)
        {
            return _connecion.QuerySingle<T>(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public Task<T> QuerySingleAsync<T>(string sql, object param = null)
        {
            return _connecion.QuerySingleAsync<T>(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public T QuerySingleOrDefault<T>(string sql, object param = null)
        {
            return _connecion.QuerySingleOrDefault<T>(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null)
        {
            return _connecion.QuerySingleOrDefaultAsync<T>(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public IEnumerable<T> QuerySql<T>(string sql, object param = null)
        {
            return _connecion.Query<T>(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public Task<IEnumerable<T>> QuerySqlAsync<T>(string sql, object param = null)
        {
            return _connecion.QueryAsync<T>(sql, param, transaction: _transaction, commandType: CommandType.Text);
        }

        public IEnumerable<T> QueryStoreProcedure<T>(string sql, object param = null)
        {
            return _connecion.Query<T>(sql, param, transaction: _transaction, commandType: CommandType.StoredProcedure);
        }

        public Task<IEnumerable<T>> QueryStoreProcedureAsync<T>(string sql, object param = null)
        {
            return _connecion.QueryAsync<T>(sql, param, transaction: _transaction, commandType: CommandType.StoredProcedure);
        }
    }
}
