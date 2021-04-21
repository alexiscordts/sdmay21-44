using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("user")]
    public partial class User
    {
        public User()
        {
            Appointment = new HashSet<Appointment>();
            Authentication = new HashSet<Authentication>();
            HoursWorked = new HashSet<HoursWorked>();
            PatientPmrPhysician = new HashSet<Patient>();
            PatientTherapist = new HashSet<Patient>();
            Permission = new HashSet<Permission>();
            TherapistEvent = new HashSet<TherapistEvent>();
        }

        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
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
        [Required]
        [Column("username")]
        [StringLength(255)]
        public string Username { get; set; }
        [Required]
        [Column("password")]
        [StringLength(255)]
        public string Password { get; set; }
        [Column("color")]
        [StringLength(255)]
        public string Color { get; set; }
        [Column("active")]
        public bool Active { get; set; }

        [InverseProperty("Therapist")]
        public virtual ICollection<Appointment> Appointment { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Authentication> Authentication { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<HoursWorked> HoursWorked { get; set; }
        [InverseProperty(nameof(Patient.PmrPhysician))]
        public virtual ICollection<Patient> PatientPmrPhysician { get; set; }
        [InverseProperty(nameof(Patient.Therapist))]
        public virtual ICollection<Patient> PatientTherapist { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Permission> Permission { get; set; }
        [InverseProperty("Therapist")]
        public virtual ICollection<TherapistEvent> TherapistEvent { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as User);
        }

        public bool Equals(User user)
        {
            if (user is null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, user))
            {
                return true;
            }


            return this.UserId == user.UserId && this.FirstName.Equals(user.FirstName) && this.MiddleName.Equals(user.MiddleName) &&
                this.LastName.Equals(user.LastName) && this.Username.Equals(user.Username);
        }

        public static bool operator ==(User lhs, User rhs)
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

        public static bool operator !=(User lhs, User rhs)
        {
            return !(lhs == rhs);
        }
    }
}
