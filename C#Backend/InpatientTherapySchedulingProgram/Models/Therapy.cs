using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("therapy")]
    public partial class Therapy
    {
        public Therapy()
        {
            Appointment = new HashSet<Appointment>();
        }

        [Key]
        [Column("adl")]
        [StringLength(255)]
        public string Adl { get; set; }
        [Column("therapy_type")]
        [StringLength(255)]
        public string TherapyType { get; set; }
        [Column("abbreviation")]
        [StringLength(255)]
        public string Abbreviation { get; set; }

        [InverseProperty("AdlNavigation")]
        public virtual ICollection<Appointment> Appointment { get; set; }
    }
}
