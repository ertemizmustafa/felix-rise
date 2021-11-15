using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Entities
{
    [Table("SCHEDULE_TARGET_PARAMETER")]
    public class ScheduleTargetParameterEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TargetId { get; set; }
        public int ParameterTypeId { get; set; }
        public bool IsCommandParameter { get; set; }
        public string TargetName { get; set; }
        public string TargetValue { get; set; }
        public int? RunTimeParameterId { get; set; }
        public string RunTimeTargetValueFormat { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }
        public string MappingName { get; set; }
    }
}
