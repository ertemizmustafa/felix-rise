using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Entities
{
    [Table("SCHEDULE_CONFIGURATION")]
    public class ScheduleConfigurationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }
        public string ConfigurationName { get; set; }
        public int ConfigurationTypeId { get; set; }
        public int ConnectionTypeId { get; set; }
    }
}
