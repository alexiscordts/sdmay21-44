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
        [Column("start_datetime", TypeName = "datetime")]
        public DateTime StartDatetime { get; set; }
        [Key]
        [Column("end_datetime", TypeName = "datetime")]
        public DateTime EndDatetime { get; set; }
        [Key]
        [Column("patient_id")]
        public int PatientId { get; set; }
        [Column("activity")]
        [StringLength(255)]
        public string Activity { get; set; }
        [Column("notes")]
        [StringLength(255)]
        public string Notes { get; set; }

        [ForeignKey(nameof(Activity))]
        [InverseProperty(nameof(PatientActivity.PatientEvent))]
        public virtual PatientActivity ActivityNavigation { get; set; }
        [ForeignKey(nameof(PatientId))]
        [InverseProperty("PatientEvent")]
        public virtual Patient Patient { get; set; }
    }
}
