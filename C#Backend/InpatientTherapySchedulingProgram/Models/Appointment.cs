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
        [Column("appointment_id")]
        public int AppointmentId { get; set; }
        [Column("start_time", TypeName = "datetime")]
        public DateTime StartTime { get; set; }
        [Column("end_time", TypeName = "datetime")]
        public DateTime EndTime { get; set; }
        [Column("therapist_id")]
        public int TherapistId { get; set; }
        [Column("patient_id")]
        public int PatientId { get; set; }
        [Column("room_number")]
        public int RoomNumber { get; set; }
        [Required]
        [Column("adl")]
        [StringLength(255)]
        public string Adl { get; set; }
        [Column("location_id")]
        public int LocationId { get; set; }
        [Column("therapist_drive_time")]
        public int? TherapistDriveTime { get; set; }
        [Column("notes")]
        [StringLength(255)]
        public string Notes { get; set; }
        [Column("active")]
        public bool Active { get; set; }

        [ForeignKey(nameof(Adl))]
        [InverseProperty(nameof(Therapy.Appointment))]
        public virtual Therapy AdlNavigation { get; set; }
        [ForeignKey(nameof(LocationId))]
        [InverseProperty("Appointment")]
        public virtual Location Location { get; set; }
        [ForeignKey(nameof(PatientId))]
        [InverseProperty("Appointment")]
        public virtual Patient Patient { get; set; }
        [ForeignKey("RoomNumber,LocationId")]
        [InverseProperty("Appointment")]
        public virtual Room Room { get; set; }
        [ForeignKey(nameof(TherapistId))]
        [InverseProperty(nameof(User.Appointment))]
        public virtual User Therapist { get; set; }
    }
}
