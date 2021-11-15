using System;
using System.Collections.Generic;
using System.Text;

namespace Felix.Data.Core.Model
{
    public sealed class BaseColumn
    {
        public string ColumnName { get; set; }
        public Type ColumnType { get; set; }
        public string ParameterName { get; set; }
        public object ParameterValue { get; set; }
        public List<object> DataValues { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsAutoIncremental { get; set; }

        public BaseColumn()
        {
            IsIdentity = false;
        }
    }
}
