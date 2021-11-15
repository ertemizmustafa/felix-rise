using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Felix.Scheduler.Entities
{
    [Table("SCHEDULE_RESOURCE")]
    public class ScheduleResourceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }
        public int ConfigurationId { get; set; }
        public string ResourceService { get; set; }
        public string ResourceServiceOutput { get; set; }
        public string ResourceCommand { get; set; }
        public string ResourceCommandOutput { get; set; }
        public int ResourceProcessTypeId { get; set; }
        public bool IsSingleValue { get; set; }
    }
}
