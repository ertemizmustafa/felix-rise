using System;
using System.Collections.Generic;
using System.Text;

namespace Felix.Common.Model
{
    [Serializable]
    public class CommonException : Exception
    {
        public CommonException(string message) : base(message) { }
        public CommonException(string message, Exception ex) : base(message, ex) { }
    }
}
