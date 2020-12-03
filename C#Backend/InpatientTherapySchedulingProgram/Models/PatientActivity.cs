using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("patient_activity")]
    public partial class PatientActivity
    {
        public PatientActivity()
        {
            PatientEvent = new HashSet<PatientEvent>();
        }

        [Key]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; }

        [InverseProperty("ActivityNavigation")]
        public virtual ICollection<PatientEvent> PatientEvent { get; set; }
    }
}
