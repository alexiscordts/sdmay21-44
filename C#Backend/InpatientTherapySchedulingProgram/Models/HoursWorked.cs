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
        [Column("hours_worked_id")]
        public int HoursWorkedId { get; set; }
        [Column("start_time", TypeName = "datetime")]
        public DateTime StartTime { get; set; }
        [Column("end_time", TypeName = "datetime")]
        public DateTime EndTime { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("active")]
        public bool Active { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("HoursWorked")]
        public virtual User User { get; set; }
    }
}
