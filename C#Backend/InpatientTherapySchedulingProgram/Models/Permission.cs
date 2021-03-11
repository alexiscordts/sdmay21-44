using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InpatientTherapySchedulingProgram.Models
{
    [Table("permission")]
    public partial class Permission : IEquatable<Permission>
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

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Permission);
        }

        public bool Equals(Permission permission)
        {
            if (permission is null)
            {
                return false;
            }

            if (Object.ReferenceEquals(this, permission))
            {
                return true;
            }

            return this.UserId == permission.UserId && this.Role.Equals(permission.Role);
        }

        public static bool operator ==(Permission lhs, Permission rhs)
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

        public static bool operator !=(Permission lhs, Permission rhs)
        {
            return !(lhs == rhs);
        }
    }
}
