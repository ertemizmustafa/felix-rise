using Felix.Data.Core.UnitOfWork;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Felix.Data.Core.Test.Test
{
    public class DapperRepositoryTest
    {
        [Fact]
        public void Query_GetValue_NonTransaction()
        {
            using var uow = new UnitOfWork<SqliteConnection>();
            var res = uow.DapperRepository().QueryFirst<object>("select 1");
            Assert.NotNull(res);
        }
    }
}
