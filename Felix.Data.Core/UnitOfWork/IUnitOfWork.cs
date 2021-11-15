using Felix.Data.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Felix.Data.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IFastCrudRepository<T> FastCrudRepository<T>();
        IDapperRepository DapperRepository();
        IAdoRepository AdoRepository();
        void Commit();
        void Rollback();
        bool IsDisposed { get; }
    }
}
