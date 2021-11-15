using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Felix.Schedule.Core.Model
{
    public sealed class ConnectedJobModel
    {
        public int JobId { get; set; }
        public int ParentJobId { get; set; }
        public Expression<Action> Action { get; set; }
    }
}
