using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Model
{
    public sealed class ScheduleProcessItemModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Description { get; set; }
        public string ResultMail { get; set; }
    }
}
