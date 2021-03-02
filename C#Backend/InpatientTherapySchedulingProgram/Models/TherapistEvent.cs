using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("therapist_event")]
    public partial class TherapistEvent
    {
        [Key]
        [Column("event_id")]
        public int EventId { get; set; }
        [Column("start_time", TypeName = "datetime")]
        public DateTime? StartTime { get; set; }
        [Column("end_time", TypeName = "datetime")]
        public DateTime? EndTime { get; set; }
        [Column("therapist_id")]
        public int? TherapistId { get; set; }
        [Column("activity_name")]
        [StringLength(255)]
        public string ActivityName { get; set; }
        [Column("notes")]
        [StringLength(255)]
        public string Notes { get; set; }

        [ForeignKey(nameof(ActivityName))]
        [InverseProperty(nameof(TherapistActivity.TherapistEvent))]
        public virtual TherapistActivity ActivityNameNavigation { get; set; }
        [ForeignKey(nameof(TherapistId))]
        [InverseProperty(nameof(User.TherapistEvent))]
        public virtual User Therapist { get; set; }
    }
}
