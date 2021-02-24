using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("room")]
    public partial class Room
    {
        [Key]
        [Column("number")]
        public int Number { get; set; }
        [Key]
        [Column("location_id")]
        public int LocationId { get; set; }

        [ForeignKey(nameof(LocationId))]
        [InverseProperty("Room")]
        public virtual Location Location { get; set; }
    }
}
