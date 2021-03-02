using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("permission")]
    public partial class Permission
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }
        [Key]
        [Column("role")]
        [StringLength(255)]
        public string Role { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Permission")]
        public virtual User User { get; set; }
    }
}
