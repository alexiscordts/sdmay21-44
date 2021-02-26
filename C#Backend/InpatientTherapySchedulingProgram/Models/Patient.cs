using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("patient")]
    public partial class Patient
    {
        public Patient()
        {
            Appointment = new HashSet<Appointment>();
            PatientEvent = new HashSet<PatientEvent>();
        }

        [Key]
        [Column("patient_id")]
        public int PatientId { get; set; }
        [Column("first_name")]
        [StringLength(255)]
        public string FirstName { get; set; }
        [Column("middle_name")]
        [StringLength(255)]
        public string MiddleName { get; set; }
        [Column("last_name")]
        [StringLength(255)]
        public string LastName { get; set; }
        [Column("address")]
        [StringLength(255)]
        public string Address { get; set; }
        [Column("phone_number")]
        [StringLength(255)]
        public string PhoneNumber { get; set; }
        [Column("location_name")]
        [StringLength(255)]
        public string LocationName { get; set; }
        [Column("start_date", TypeName = "date")]
        public DateTime? StartDate { get; set; }
        [Column("pmr_physician")]
        [StringLength(255)]
        public string PmrPhysician { get; set; }

        public virtual Location LocationNameNavigation { get; set; }
        [InverseProperty("Patient")]
        public virtual ICollection<Appointment> Appointment { get; set; }
        [InverseProperty("Patient")]
        public virtual ICollection<PatientEvent> PatientEvent { get; set; }
    }
}
