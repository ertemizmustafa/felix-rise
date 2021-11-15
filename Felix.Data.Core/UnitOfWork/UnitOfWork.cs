using Felix.Data.Core.Configuration;
using Felix.Data.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Felix.Data.Core.UnitOfWork
{
    public class UnitOfWork<TConnection> : IUnitOfWork where TConnection : IDbConnection, new()
    {

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public bool IsDisposed { get; private set; } = false;
        public IFastCrudRepository<T> FastCrudRepository<T>() => new FastCrudRepository<T>(_connection, _transaction);
        public IDapperRepository DapperRepository() => new DapperRepository(_connection, _transaction);
        public IAdoRepository AdoRepository() => new AdoRepository(_connection, _transaction);

        public UnitOfWork(string connectionString = "", bool transactional = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _connection = OpenConnection(connectionString);

            if (transactional)
                _transaction = _connection.BeginTransaction(isolationLevel);
        }

        private IDbConnection OpenConnection(string connectionString)
        {
            object _connectionString;
            if (!string.IsNullOrEmpty(connectionString))
            {
                _connectionString = connectionString;
            }
            else
            {
                CacheExtensions.ConnectionString.TryGetValue("ConnectionString", out _connectionString);
            }

            var connection = new TConnection
            {
                ConnectionString = (string)_connectionString
            };

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while connecting to the database. See innerException for details.", ex);
            }

            return connection;

        }

        public void Commit() => _transaction?.Commit();
        public void Rollback() => _transaction?.Rollback();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (disposing)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
            }

            _transaction = null;
            _connection = null;

            IsDisposed = true;
        }

    }
}
