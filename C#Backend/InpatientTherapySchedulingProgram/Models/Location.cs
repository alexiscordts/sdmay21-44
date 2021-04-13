using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("location")]
    public partial class Location : IEquatable<Location>
    {
        public Location()
        {
            Appointment = new HashSet<Appointment>();
            Patient = new HashSet<Patient>();
            Room = new HashSet<Room>();
        }

        [Key]
        [Column("location_id")]
        public int LocationId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [Column("address")]
        [StringLength(255)]
        public string Address { get; set; }
        [Required]
        [Column("phone_number")]
        [StringLength(15)]
        public string PhoneNumber { get; set; }
        [Column("active")]
        public bool Active { get; set; }

        [InverseProperty("Location")]
        public virtual ICollection<Appointment> Appointment { get; set; }
        [InverseProperty("Location")]
        public virtual ICollection<Patient> Patient { get; set; }
        [InverseProperty("Location")]
        public virtual ICollection<Room> Room { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Location);
        }

        public bool Equals(Location location)
        {
            if (location is null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, location))
            {
                return true;
            }

            return this.LocationId == location.LocationId && this.Name.Equals(location.Name);
        }

        public static bool operator ==(Location lhs, Location rhs)
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

        public static bool operator !=(Location lhs, Location rhs)
        {
            return !(lhs == rhs);
        }
    }
}
