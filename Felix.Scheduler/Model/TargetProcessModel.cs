using Felix.Scheduler.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Process
{
    public class TargetProcessModel
    {
        public int TargetId { get; set; }
        public string Description { get; set; }
        public IEnumerable<dynamic> DataList { get; set; }
        public bool DeletePreviousData { get; set; }
        public string Service { get; set; }
        public string ServiceOutput { get; set; }
        public string BeforeCommand { get; set; }
        public int? BeforeCommandProcessTypeId { get; set; }
        public string Command { get; set; }
        public int? CommandProcessTypeId { get; set; }
        public IEnumerable<ScheduleTargetParameterEntity> TargetParameterMappings { get; set; }
        public IDictionary<string,object> CommandParameters { get; set; }
        public string CommandOutputName { get; set; }
        public string AfterCommand { get; set; }
        public int? AfterCommandProcessTypeId { get; set; }
        public List<ScheduleParameterTypeEntity> ScheduleParameterTypes { get; set; }
    }
}
