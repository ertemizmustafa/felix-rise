using System;
using System.Collections.Generic;
using System.Text;

namespace Felix.Data.Core.Model
{
    public interface IBaseTable
    {
        string GenerateInsertScript(bool addDoubleQuote = false);
        string GenerateDeleteScript(bool addDoubleQuote = false, string condition = "");
    }
}
