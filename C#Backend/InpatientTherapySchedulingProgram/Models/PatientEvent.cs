using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("patient_event")]
    public partial class PatientEvent
    {
        [Key]
        [Column("event_id")]
        public int EventId { get; set; }
        [Column("start_time", TypeName = "datetime")]
        public DateTime? StartTime { get; set; }
        [Column("end_time", TypeName = "datetime")]
        public DateTime? EndTime { get; set; }
        [Column("patient_id")]
        public int? PatientId { get; set; }
        [Column("activity_name")]
        [StringLength(255)]
        public string ActivityName { get; set; }
        [Column("notes")]
        [StringLength(255)]
        public string Notes { get; set; }

        [ForeignKey(nameof(ActivityName))]
        [InverseProperty(nameof(PatientActivity.PatientEvent))]
        public virtual PatientActivity ActivityNameNavigation { get; set; }
        [ForeignKey(nameof(PatientId))]
        [InverseProperty("PatientEvent")]
        public virtual Patient Patient { get; set; }
    }
}
