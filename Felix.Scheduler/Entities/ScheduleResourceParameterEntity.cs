using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Entities
{
    [Table("SCHEDULE_RESOURCE_PARAMETER")]
    public class ScheduleResourceParameterEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int? RunTimeParameterId { get; set; }
        public string RunTimeParameterFormat { get; set; }
    }
}
