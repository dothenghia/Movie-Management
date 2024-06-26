﻿using System;
using System.Collections.Generic;

namespace MovieManagement.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Movies = new HashSet<Movie>();
        }

        public string? GenreName { get; set; }
        public int GenreId { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
