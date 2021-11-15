using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper.FastCrud.Configuration.StatementOptions.Builders;

namespace Felix.Data.Core.Repository
{
    public interface IFastCrudRepository<T>
    {
        IEnumerable<T> Find(Action<IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<T>> statementOptions = null);
        Task<IEnumerable<T>> FindAsync(Action<IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<T>> statementOptions = null);
        T Get(T entityKeys, Action<ISelectSqlSqlStatementOptionsBuilder<T>> statementOptions = null);
        Task<T> GetAsync(T entityKeys, Action<ISelectSqlSqlStatementOptionsBuilder<T>> statementOptions = null);
        int BulkDelete(Action<IConditionalBulkSqlStatementOptionsBuilder<T>> statementOptions = null);
        Task<int> BulkDeleteAsync(Action<IConditionalBulkSqlStatementOptionsBuilder<T>> statementOptions = null);
        int BulkUpdate(T updateData, Action<IConditionalBulkSqlStatementOptionsBuilder<T>> statementOptions = null);
        Task<int> BulkUpdateAsync(T updateData, Action<IConditionalBulkSqlStatementOptionsBuilder<T>> statementOptions = null);
        int Count(Action<IConditionalSqlStatementOptionsBuilder<T>> statementOptions = null);
        Task<int> CountAsync(Action<IConditionalSqlStatementOptionsBuilder<T>> statementOptions = null);
        bool Delete(T entityToDelete, Action<IStandardSqlStatementOptionsBuilder<T>> statementOptions = null);
        Task<bool> DeleteAsync(T entityToDelete, Action<IStandardSqlStatementOptionsBuilder<T>> statementOptions = null);
        void Insert(T entityToInsert, Action<IStandardSqlStatementOptionsBuilder<T>> statementOptions = null);
        Task InsertAsync(T entityToInsert, Action<IStandardSqlStatementOptionsBuilder<T>> statementOptions = null);
        bool Update(T entityToUpdate, Action<IStandardSqlStatementOptionsBuilder<T>> statementOptions = null);
        Task<bool> UpdateAsync(T entityToUpdate, Action<IStandardSqlStatementOptionsBuilder<T>> statementOptions = null);
    }
}
