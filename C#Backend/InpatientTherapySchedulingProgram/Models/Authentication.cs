using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("authentication")]
    public partial class Authentication
    {
        [Key]
        [Column("authentication_id")]
        public int AuthenticationId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("token")]
        [StringLength(255)]
        public string Token { get; set; }
        [Column("login_time", TypeName = "datetime")]
        public DateTime? LoginTime { get; set; }
        [Column("expiration_time", TypeName = "datetime")]
        public DateTime? ExpirationTime { get; set; }
        [Column("active")]
        public bool Active { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Authentication")]
        public virtual User User { get; set; }
    }
}
