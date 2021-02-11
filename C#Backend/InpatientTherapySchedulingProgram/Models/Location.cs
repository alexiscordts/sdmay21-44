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
            RoomNumber = new HashSet<RoomNumber>();
        }

        [Key]
        [Column("lid")]
        public int Lid { get; set; }
        [Column("name")]
        [StringLength(255)]
        public string Name { get; set; }

        [InverseProperty("L")]
        public virtual ICollection<Appointment> Appointment { get; set; }
        [InverseProperty("L")]
        public virtual ICollection<RoomNumber> RoomNumber { get; set; }

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

            if(Object.ReferenceEquals(this, location))
            {
                return true;
            }

            return this.Lid == location.Lid && this.Name.Equals(location.Name);
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
