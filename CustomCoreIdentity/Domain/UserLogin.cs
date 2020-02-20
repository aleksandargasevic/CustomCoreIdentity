using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCoreIdentity.Domain
{
    public partial class UserLogin
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(UserId))]
        public Guid UserId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }

        public virtual User User { get; set; }
    }
}
