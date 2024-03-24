using System;
using System.Collections.Generic;

namespace MovieManagement.Models
{
    public partial class Person
    {
        public Person()
        {
            Contributors = new HashSet<Contributor>();
        }

        public string? Fullname { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Biography { get; set; }
        public int PersonId { get; set; }

        public virtual ICollection<Contributor> Contributors { get; set; }

    }
}
