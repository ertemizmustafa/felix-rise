using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Model
{
    public sealed class ResourceProcessModel
    {
        public int ResourceId { get; set; }
        public int ProcessType { get; set; }
        public string Description { get; set; }
        public string Service { get; set; }
        public string ServiceOutput { get; set; }
        public string Command { get; set; }
        public IDictionary<string,object> CommandParameters { get; set; }
        public string CommandOutputName { get; set; }
        public bool IsSingleValue { get; set; }

    }
}
