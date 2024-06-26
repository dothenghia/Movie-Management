﻿using System;
using System.Collections.Generic;

namespace MovieManagement.Models
{
    public partial class AgeCertificate
    {
        public AgeCertificate()
        {
            Movies = new HashSet<Movie>();
        }

        public string? DisplayContent { get; set; }
        public int? RequireAge { get; set; }
        public int AgeCertificateId { get; set; }
        public string? BackgroundColor { get; set; }
        public string? ForegroundColor { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
