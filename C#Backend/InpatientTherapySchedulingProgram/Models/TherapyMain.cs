using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("therapy_main")]
    public partial class TherapyMain
    {
        public TherapyMain()
        {
            Therapy = new HashSet<Therapy>();
        }

        [Key]
        [Column("type")]
        [StringLength(255)]
        public string Type { get; set; }
        [Required]
        [Column("abbreviation")]
        [StringLength(255)]
        public string Abbreviation { get; set; }
        [Column("active")]
        public bool Active { get; set; }

        [InverseProperty("TypeNavigation")]
        public virtual ICollection<Therapy> Therapy { get; set; }
    }
}
