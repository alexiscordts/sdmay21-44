using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("room")]
    public partial class Room : IEquatable<Room>
    {
        public Room()
        {
            Appointment = new HashSet<Appointment>();
            Patient = new HashSet<Patient>();
        }

        [Key]
        [Column("number")]
        public int Number { get; set; }
        [Key]
        [Column("location_id")]
        public int LocationId { get; set; }
        [Column("active")]
        public bool Active { get; set; }

        [ForeignKey(nameof(LocationId))]
        [InverseProperty("Room")]
        public virtual Location Location { get; set; }
        [InverseProperty("Room")]
        public virtual ICollection<Appointment> Appointment { get; set; }
        [InverseProperty("Room")]
        public virtual ICollection<Patient> Patient { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Room);
        }

        public bool Equals(Room room)
        {
            if (room is null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, room))
            {
                return true;
            }

            return this.Number == room.Number && this.LocationId == room.LocationId;
        }

        public static bool operator ==(Room lhs, Room rhs) 
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
        
        public static bool operator !=(Room lhs, Room rhs)
        {
            return !(lhs == rhs);
        }
    }
}
