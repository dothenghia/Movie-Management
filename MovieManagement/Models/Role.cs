using System;
using System.Collections.Generic;

namespace MovieManagement.Models
{
    public partial class Role
    {
        public Role()
        {
            Contributors = new HashSet<Contributor>();
        }

        public string? RoleName { get; set; }
        public int RoleId { get; set; }

        public virtual ICollection<Contributor> Contributors { get; set; }
    }
}
