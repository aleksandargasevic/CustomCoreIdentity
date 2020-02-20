using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCoreIdentity.Domain
{
    public partial class Role
    {
        public Role(string name)
        {
            this.Users = new HashSet<UserRole>();
            this.Name = name;
        }
        public Role()
        {
            this.Users = new HashSet<UserRole>();
        }

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ConcurrencyStamp { get; set; }

        public virtual ICollection<UserRole> Users { get; set; }
    }
}
