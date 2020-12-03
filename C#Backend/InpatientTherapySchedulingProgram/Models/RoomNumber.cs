using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("room_number")]
    public partial class RoomNumber
    {
        [Key]
        [Column("number")]
        public int Number { get; set; }
        [Key]
        [Column("lid")]
        public int Lid { get; set; }

        [ForeignKey(nameof(Lid))]
        [InverseProperty(nameof(Location.RoomNumber))]
        public virtual Location L { get; set; }
    }
}
