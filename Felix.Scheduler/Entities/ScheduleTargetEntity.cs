using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Entities
{
    [Table("SCHEDULE_TARGET")]
    public class ScheduleTargetEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }
        public int ConfigurationId { get; set; }
        public string TargetService { get; set; }
        public string TargetServiceOutput { get; set; }
        public string TargetCommand { get; set; }
        public int? TargetCommandProcessTypeId { get; set; }
        public string TargetCommandOutput { get; set; }
        public bool DeletePreviousData { get; set; }
        public bool IsSubTarget { get; set; }
        public bool UseAsResourceForAfterProcess { get; set; }
        public int? BeforeTargetId { get; set; }
        public int? AfterTargetId { get; set; }
       
    }
}
