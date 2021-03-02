using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("patient_activity")]
    public partial class PatientActivity : IEquatable<PatientActivity>
    {
        public PatientActivity()
        {
            PatientEvent = new HashSet<PatientEvent>();
        }

        [Key]
        [Column("activity_name")]
        [StringLength(255)]
        public string ActivityName { get; set; }

        [InverseProperty("ActivityNameNavigation")]
        public virtual ICollection<PatientEvent> PatientEvent { get; set; }

        public bool Equals(PatientActivity patientActivity)
        {
            if (patientActivity is null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, patientActivity))
            {
                return true;
            }

            return this.Name == patientActivity.Name;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PatientActivity);
        }

        public static bool operator ==(PatientActivity lhs, PatientActivity rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(PatientActivity lhs, PatientActivity rhs)
        {
            return !(lhs == rhs);
        }
    }
}
