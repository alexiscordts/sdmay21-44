using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("therapist_activity")]
    public partial class TherapistActivity
    {
        public TherapistActivity()
        {
            TherapistEvent = new HashSet<TherapistEvent>();
        }

        [Key]
        [Column("activity_name")]
        [StringLength(255)]
        public string ActivityName { get; set; }
        [Column("isProductive")]
        public bool? IsProductive { get; set; }

        [InverseProperty("ActivityNameNavigation")]
        public virtual ICollection<TherapistEvent> TherapistEvent { get; set; }
    }
}
