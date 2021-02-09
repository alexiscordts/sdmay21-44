using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("location")]
    public partial class Location
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
    }
}
