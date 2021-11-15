using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Felix.Data.Core.Test.Helpers
{
    public static class TestHelper
    {
        public const string TableName = "Test";

        public static string GenerateCreateTableScript()
        {
            return $"CREATE TABLE {TableName} (Id INTEGER PRIMARY KEY, Value1 TEXT, Value2 TEXT, Value3 TEXT)";
        }
    }

    [Table(TestHelper.TableName)]
    public class Test
    {
        [Key]
        public int Id { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
    }
}
