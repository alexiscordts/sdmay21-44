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
        [Required]
        [Column("type")]
        [StringLength(255)]
        public string Type { get; set; }
        [Required]
        [Column("abbreviation")]
        [StringLength(255)]
        public string Abbreviation { get; set; }
        [Column("active")]
        public bool Active { get; set; }

        [ForeignKey(nameof(Type))]
        [InverseProperty(nameof(TherapyMain.Therapy))]
        public virtual TherapyMain TypeNavigation { get; set; }
        [InverseProperty("AdlNavigation")]
        public virtual ICollection<Appointment> Appointment { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Therapy);
        }

        public bool Equals(Therapy therapy)
        {
            if (Object.ReferenceEquals(therapy, null))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, therapy))
            {
                return true;
            }

            if (this.GetType() != therapy.GetType())
            {
                return false;
            }

            return (this.Adl == therapy.Adl) && (this.Abbreviation == therapy.Abbreviation) && (this.Type == therapy.Type);
        }

        public static bool operator ==(Therapy lhs, Therapy rhs)
        {
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
                {
                    return true;
                }

                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(Therapy lhs, Therapy rhs)
        {
            return !(lhs == rhs);
        }
    }
}
