using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Felix.Data.Core.Model
{
    public abstract class BaseTable : IBaseTable
    {
        public string TableName { get; set; }
        public List<BaseColumn> Columns { get; set; }
        public bool DeletePreviousData { get; set; }

        public virtual string GenerateDeleteScript(bool addDoubleQuote = false, string condition = "")
        {
            var generatedQuery = new StringBuilder();

            if (!string.IsNullOrEmpty(TableName))
            {
                generatedQuery.Append(string.Format("delete from {0}", addDoubleQuote ? string.Format("\"{0}\"", TableName) : TableName));

                if (!string.IsNullOrEmpty(condition))
                    generatedQuery.Append(!condition.ToLower().Contains("where") ? string.Format("{0} {1}", "where", condition) : condition);
            }

            return generatedQuery.ToString();
        }

        public string GenerateInsertScript(bool addDoubleQuote = false)
        {

            var generatedQuery = new StringBuilder();

            if (!string.IsNullOrEmpty(TableName))
            {
                generatedQuery.Append(string.Format("insert into {0} ({1}) values ({2})", addDoubleQuote ? string.Format("\"{0}\"", TableName) : TableName, string.Join(",", Columns.Where(x => !x.IsAutoIncremental || (x.IsIdentity = false)).Select(x => addDoubleQuote ? string.Format("\"{0}\"", x.ColumnName) : TableName).ToArray()), string.Join(",", Columns.Where(x => !x.IsAutoIncremental || (x.IsIdentity = false)).Select(x => string.Concat(":", x.ParameterName)))));
            }

            return generatedQuery.ToString();
        }
    }
}
