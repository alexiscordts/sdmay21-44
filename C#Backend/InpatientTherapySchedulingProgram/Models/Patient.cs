using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("patient")]
    public partial class Patient : IEquatable<Patient>
    {
        public Patient()
        {
            Appointment = new HashSet<Appointment>();
        }

        [Key]
        [Column("patient_id")]
        public int PatientId { get; set; }
        [Required]
        [Column("first_name")]
        [StringLength(255)]
        public string FirstName { get; set; }
        [Column("middle_name")]
        [StringLength(255)]
        public string MiddleName { get; set; }
        [Column("last_name")]
        [StringLength(255)]
        public string LastName { get; set; }
        [Column("address")]
        [StringLength(255)]
        public string Address { get; set; }
        [Column("phone_number")]
        [StringLength(255)]
        public string PhoneNumber { get; set; }
        [Column("room_number")]
        public int RoomNumber { get; set; }
        [Column("location_id")]
        public int LocationId { get; set; }
        [Column("start_date", TypeName = "date")]
        public DateTime? StartDate { get; set; }
        [Column("pmr_physician_id")]
        public int PmrPhysicianId { get; set; }
        [Column("active")]
        public bool Active { get; set; }

        [ForeignKey(nameof(LocationId))]
        [InverseProperty("Patient")]
        public virtual Location Location { get; set; }
        [ForeignKey(nameof(PmrPhysicianId))]
        [InverseProperty(nameof(User.Patient))]
        public virtual User PmrPhysician { get; set; }
        [ForeignKey("RoomNumber,LocationId")]
        [InverseProperty("Patient")]
        public virtual Room Room { get; set; }
        [InverseProperty("Patient")]
        public virtual ICollection<Appointment> Appointment { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Patient);
        }

        public bool Equals(Patient patient)
        {
            if (patient is null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, patient))
            {
                return true;
            }

            return this.PatientId == patient.PatientId && this.FirstName.Equals(patient.FirstName) && this.MiddleName.Equals(patient.MiddleName) &&
                this.LastName.Equals(patient.LastName) && this.PmrPhysicianId == patient.PmrPhysicianId;
        }

        public static bool operator ==(Patient lhs, Patient rhs)
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

        public static bool operator !=(Patient lhs, Patient rhs)
        {
            return !(lhs == rhs);
        }
    }
}
