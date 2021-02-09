using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("hours_worked")]
    public partial class HoursWorked
    {
        [Key]
        [Column("start_datetime", TypeName = "datetime")]
        public DateTime StartDatetime { get; set; }
        [Key]
        [Column("end_datetime", TypeName = "datetime")]
        public DateTime EndDatetime { get; set; }
        [Key]
        [Column("uid")]
        public int Uid { get; set; }

        [ForeignKey(nameof(Uid))]
        [InverseProperty(nameof(User.HoursWorked))]
        public virtual User U { get; set; }
    }
}
