using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("therapy_main")]
    public partial class TherapyMain : IEquatable<TherapyMain>
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

        public override bool Equals(object obj)
        {
            return this.Equals(obj as TherapyMain);
        }

        public bool Equals(TherapyMain therapyMain)
        {
            if (therapyMain is null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, therapyMain))
            {
                return true;
            }

            return this.Type.Equals(therapyMain.Type) && this.Abbreviation.Equals(therapyMain.Abbreviation);
        }

        public static bool operator ==(TherapyMain lhs, TherapyMain rhs)
        {
            if (Object.ReferenceEquals(lhs, rhs))
            {
                return true;
            }
            else if (lhs is null)
            {
                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(TherapyMain lhs, TherapyMain rhs)
        {
            return !(lhs == rhs);
        }
    }
}
