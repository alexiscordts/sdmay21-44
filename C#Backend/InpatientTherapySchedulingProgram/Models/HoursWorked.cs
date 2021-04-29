using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("hours_worked")]
    public partial class HoursWorked : IEquatable<HoursWorked>
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

        public override bool Equals(object obj)
        {
            return this.Equals(obj as HoursWorked);
        }
        public bool Equals(HoursWorked hoursWorked)
        {
            if (hoursWorked is null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, hoursWorked))
            {
                return true;
            }

            //return this.UserId == hoursWorked.UserId && this.HoursWorkedId.Equals(hoursWorked.HoursWorkedId);
            return HoursWorkedId == hoursWorked.HoursWorkedId;
        }

        public static bool operator ==(HoursWorked lhs, HoursWorked rhs)
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

        public static bool operator !=(HoursWorked lhs, HoursWorked rhs)
        {
            return !(lhs == rhs);
        }
    }
}
