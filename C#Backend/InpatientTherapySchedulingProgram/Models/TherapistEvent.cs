using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("therapist_event")]
    public partial class TherapistEvent : IEquatable<TherapistEvent>
    {
        [Key]
        [Column("event_id")]
        public int EventId { get; set; }
        [Column("start_time", TypeName = "datetime")]
        public DateTime StartTime { get; set; }
        [Column("end_time", TypeName = "datetime")]
        public DateTime EndTime { get; set; }
        [Column("therapist_id")]
        public int TherapistId { get; set; }
        [Column("activity_name")]
        [StringLength(255)]
        public string ActivityName { get; set; }
        [Column("notes")]
        [StringLength(255)]
        public string Notes { get; set; }
        [Column("active")]
        public bool Active { get; set; }

        [ForeignKey(nameof(TherapistId))]
        [InverseProperty(nameof(User.TherapistEvent))]
        public virtual User Therapist { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as TherapistEvent);
        }

        public bool Equals(TherapistEvent therapistEvent)
        {
            if (therapistEvent is null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, therapistEvent))
            {
                return true;
            }

            return this.EventId == therapistEvent.EventId && this.TherapistId == therapistEvent.TherapistId
                && this.ActivityName.Equals(therapistEvent.ActivityName) && this.StartTime.Equals(therapistEvent.StartTime)
                && this.EndTime.Equals(therapistEvent.EndTime);
        }

        public static bool operator ==(TherapistEvent lhs, TherapistEvent rhs)
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

        public static bool operator !=(TherapistEvent lhs, TherapistEvent rhs)
        {
            return !(lhs == rhs);
        }
    }
}
