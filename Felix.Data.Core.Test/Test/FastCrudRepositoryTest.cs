using Felix.Data.Core.Test.Helpers;
using Felix.Data.Core.UnitOfWork;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Felix.Data.Core.Test.Test
{
    public class FastCrudRepositoryTest
    {
        public void Insert_Value_NonTransaction()
        {
            using var uow = new UnitOfWork<SqliteConnection>();

            uow.DapperRepository().ExecuteSql(TestHelper.GenerateCreateTableScript());

            var insertItem = new Helpers.Test { Value1 = "Val1", Value2 = "Val2", Value3 = "Val3" };
            uow.FastCrudRepository<Helpers.Test>().Insert(insertItem);

            Assert.True(insertItem.Id > 0);
        }
    }
}
