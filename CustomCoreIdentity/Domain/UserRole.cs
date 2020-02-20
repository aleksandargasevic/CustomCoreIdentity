using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCoreIdentity.Domain
{
    public class UserRole
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(UserId))]
        public Guid UserId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public Guid RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
