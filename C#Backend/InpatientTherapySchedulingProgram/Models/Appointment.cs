using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("appointment")]
    public partial class Appointment
    {
        [Key]
        [Column("start_datetime", TypeName = "datetime")]
        public DateTime StartDatetime { get; set; }
        [Key]
        [Column("end_datetime", TypeName = "datetime")]
        public DateTime EndDatetime { get; set; }
        [Key]
        [Column("therapist_id")]
        public int TherapistId { get; set; }
        [Key]
        [Column("patient_id")]
        public int PatientId { get; set; }
        [Column("room_number")]
        public int? RoomNumber { get; set; }
        [Column("adl")]
        [StringLength(255)]
        public string Adl { get; set; }
        [Column("lid")]
        public int? Lid { get; set; }
        [Column("therapist_drive_time")]
        public int? TherapistDriveTime { get; set; }
        [Column("notes")]
        [StringLength(255)]
        public string Notes { get; set; }

        [ForeignKey(nameof(Adl))]
        [InverseProperty(nameof(Therapy.Appointment))]
        public virtual Therapy AdlNavigation { get; set; }
        [ForeignKey(nameof(Lid))]
        [InverseProperty(nameof(Location.Appointment))]
        public virtual Location L { get; set; }
        [ForeignKey(nameof(PatientId))]
        [InverseProperty("Appointment")]
        public virtual Patient Patient { get; set; }
        [ForeignKey(nameof(TherapistId))]
        [InverseProperty(nameof(User.Appointment))]
        public virtual User Therapist { get; set; }
    }
}
