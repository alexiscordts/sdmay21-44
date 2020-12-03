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
        [Column("id")]
        public int Id { get; set; }
        [Key]
        [Column("role")]
        [StringLength(255)]
        public string Role { get; set; }
        [Column("type")]
        [StringLength(10)]
        public string Type { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(User.Permission))]
        public virtual User IdNavigation { get; set; }
    }
}
