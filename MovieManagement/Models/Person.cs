using System;
using System.Collections.Generic;

namespace MovieManagement.Models
{
    public partial class Person
    {
        public Person()
        {
            Movies = new HashSet<Movie>();
            MoviesNavigation = new HashSet<Movie>();
        }

        public string Fullname { get; set; }

        public string AvatarUrl { get; set; }

        public string Biography { get; set; }

        public int PersonId { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

        public virtual ICollection<Movie> MoviesNavigation { get; set; }
    }
}
