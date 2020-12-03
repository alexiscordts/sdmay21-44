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
        [Column("start_datetime", TypeName = "datetime")]
        public DateTime StartDatetime { get; set; }
        [Key]
        [Column("end_datetime", TypeName = "datetime")]
        public DateTime EndDatetime { get; set; }
        [Key]
        [Column("therapist_id")]
        public int TherapistId { get; set; }
        [Column("activity")]
        [StringLength(255)]
        public string Activity { get; set; }
        [Column("notes")]
        [StringLength(255)]
        public string Notes { get; set; }

        [ForeignKey(nameof(Activity))]
        [InverseProperty(nameof(TherapistActivity.TherapistEvent))]
        public virtual TherapistActivity ActivityNavigation { get; set; }
        [ForeignKey(nameof(TherapistId))]
        [InverseProperty(nameof(User.TherapistEvent))]
        public virtual User Therapist { get; set; }
    }
}
