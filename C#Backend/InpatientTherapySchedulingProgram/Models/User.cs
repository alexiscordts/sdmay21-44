using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("user")]
    public partial class User : IEquatable<User>
    {
        public User()
        {
            Appointment = new HashSet<Appointment>();
            HoursWorked = new HashSet<HoursWorked>();
            Permission = new HashSet<Permission>();
            TherapistEvent = new HashSet<TherapistEvent>();
        }

        [Key]
        [Column("uid")]
        public int Uid { get; set; }
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
        [Column("username")]
        [StringLength(255)]
        public string Username { get; set; }
        [Column("password")]
        [StringLength(255)]
        public string Password { get; set; }

        [InverseProperty("Therapist")]
        public virtual ICollection<Appointment> Appointment { get; set; }
        [InverseProperty("U")]
        public virtual ICollection<HoursWorked> HoursWorked { get; set; }
        [InverseProperty("IdNavigation")]
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

            return this.Uid == user.Uid && this.FirstName == user.FirstName && this.MiddleName == user.MiddleName &&
                this.LastName == user.LastName && this.Username == user.Username;
        }

        public static bool operator ==(User lhs, User rhs)
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

        public static bool operator !=(User lhs, User rhs)
        {
            return !(lhs == rhs);
        }
    }
}
