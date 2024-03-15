using System;
using System.Collections.Generic;

namespace MovieManagement.Models
{
    public partial class Movie
    {
        public Movie()
        {
            ShowTimes = new HashSet<ShowTime>();
            People = new HashSet<Person>();
        }

        public string Title { get; set; }
        public int? Duration { get; set; }
        public int? PublishYear { get; set; }
        public double? ImdbScore { get; set; }
        public int? AgeCertificateId { get; set; }
        public int? DirectorId { get; set; }
        public int MovieId { get; set; }
        public bool? IsGoldenHour { get; set; }
        public bool? IsBlockbuster { get; set; }
        public string PosterUrl { get; set; }
        public string TrailerUrl { get; set; }
        public string Description { get; set; }
        public int? GenreId { get; set; }

        public virtual AgeCertificate AgeCertificate { get; set; }
        public virtual Person Director { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual ICollection<ShowTime> ShowTimes { get; set; }

        public virtual ICollection<Person> People { get; set; }
    }
}
