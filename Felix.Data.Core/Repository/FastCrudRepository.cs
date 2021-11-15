using Dapper.FastCrud;
using Dapper.FastCrud.Configuration.StatementOptions.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Felix.Data.Core.Repository
{
    public class FastCrudRepository<T> : IFastCrudRepository<T>
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        internal FastCrudRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public int BulkDelete(Action<IConditionalBulkSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            return _connection.BulkDelete<T>(statementOptions);
        }

        public Task<int> BulkDeleteAsync(Action<IConditionalBulkSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            return _connection.BulkDeleteAsync<T>(statementOptions);
        }

        public int BulkUpdate(T updateData, Action<IConditionalBulkSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            return _connection.BulkUpdate<T>(updateData, statementOptions);
        }

        public Task<int> BulkUpdateAsync(T updateData, Action<IConditionalBulkSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            return _connection.BulkUpdateAsync<T>(updateData, statementOptions);
        }

        public int Count(Action<IConditionalSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            return _connection.Count<T>(statementOptions);
        }

        public Task<int> CountAsync(Action<IConditionalSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            return _connection.CountAsync<T>(statementOptions);
        }

        public bool Delete(T entityToDelete, Action<IStandardSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            return _connection.Delete<T>(entityToDelete, statementOptions);
        }

        public Task<bool> DeleteAsync(T entityToDelete, Action<IStandardSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            return _connection.DeleteAsync<T>(entityToDelete, statementOptions);
        }

        public IEnumerable<T> Find(Action<IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            return _connection.Find<T>(statementOptions);
        }

        public Task<IEnumerable<T>> FindAsync(Action<IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            return _connection.FindAsync<T>(statementOptions);
        }

        public T Get(T entityKeys, Action<ISelectSqlSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            return _connection.Get<T>(entityKeys, statementOptions);
        }

        public Task<T> GetAsync(T entityKeys, Action<ISelectSqlSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            return _connection.GetAsync<T>(entityKeys, statementOptions);
        }

        public void Insert(T entityToInsert, Action<IStandardSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            _connection.Insert<T>(entityToInsert, statementOptions);
        }

        public async Task InsertAsync(T entityToInsert, Action<IStandardSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            await _connection.InsertAsync<T>(entityToInsert, statementOptions);
        }

        public bool Update(T entityToUpdate, Action<IStandardSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            return _connection.Update<T>(entityToUpdate, statementOptions);
        }

        public Task<bool> UpdateAsync(T entityToUpdate, Action<IStandardSqlStatementOptionsBuilder<T>> statementOptions = null)
        {
            if (_transaction != null)
                statementOptions += s => s.AttachToTransaction(_transaction);

            return _connection.UpdateAsync<T>(entityToUpdate, statementOptions);
        }
    }
}
