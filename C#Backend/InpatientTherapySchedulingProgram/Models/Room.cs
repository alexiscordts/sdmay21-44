using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("room")]
    public partial class Room
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
    }
}
