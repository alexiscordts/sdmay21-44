﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("therapist_activity")]
    public partial class TherapistActivity : IEquatable<TherapistActivity>
    {
        public TherapistActivity()
        {
            TherapistEvent = new HashSet<TherapistEvent>();
        }

        [Key]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; }
        [Column("isProductive")]
        public bool? IsProductive { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as TherapistActivity);
        }

        public bool Equals(TherapistActivity therapistActivity)
        {
            if(Object.ReferenceEquals(therapistActivity, null))
            {
                return false;
            }

            if(Object.ReferenceEquals(this, therapistActivity))
            {
                return true;
            }

            if(this.GetType() != therapistActivity.GetType())
            {
                return false;
            }

            return (this.Name == therapistActivity.Name) && (this.IsProductive == therapistActivity.IsProductive);
        }

        [InverseProperty("ActivityNavigation")]
        public virtual ICollection<TherapistEvent> TherapistEvent { get; set; }
    }
}
