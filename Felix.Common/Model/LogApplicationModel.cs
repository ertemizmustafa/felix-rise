using Felix.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Felix.Common.Model
{
    public sealed class LogApplicationModel
    {
        public string Message { get; set; }
        public string TransactionId => ApplicationContext.TransactionId;
        public string CreatedBy => ApplicationContext.UserName;

        public DateTime ActionStartTime { get; set; }

        public string ActionCallerIp => ApplicationContext.ClientIp;

    }
}
