using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Entities
{
    [Table("SCHEDULE_JOB")]
    public class ScheduleJobEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Description { get; set; }
        public int ApplicationId { get; set; }
        public int? ResourceId { get; set; }
        public int TargetId { get; set; }
        public string Cron { get; set; }
        public int? RetryInterval { get; set; }
        public bool CanContinueIfFails { get; set; }
        public bool SkipTargetIfResourceDataEmpty { get; set; }
        public bool IsActive { get; set; }
        public string ResultMail { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateDate { get; set; }
    }
}
