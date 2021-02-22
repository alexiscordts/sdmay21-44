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

        [InverseProperty("Location")]
        public virtual ICollection<Appointment> Appointment { get; set; }
        public virtual ICollection<Patient> Patient { get; set; }
        [InverseProperty("Location")]
        public virtual ICollection<Room> Room { get; set; }
    }
}
