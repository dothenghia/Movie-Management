using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MovieManagement.ViewModels
{
    public class Admin_FilmInfo_ViewModel : ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public ObservableCollection<dynamic> FilmInfo { get; set; }
        public ObservableCollection<dynamic> Actors { get; set; }
        public ObservableCollection<dynamic> Genres { get; set; }
        public List<string> GenresList { get; set; } = new List<string>();
        public ObservableCollection<dynamic> AgeCertificates { get; set; }
        public List<string> AgeCertificatesList { get; set; } = new List<string>();
        public Admin_FilmInfo_ViewModel()
        {
            var movies = (from m in _context.Movies
                          join a in _context.AgeCertificates on m.AgeCertificateId equals a.AgeCertificateId
                          join g in _context.Genres on m.GenreId equals g.GenreId
                          select new
                          {
                              MovieId = m.MovieId,
                              Title = m.Title,
                              Duration = m.Duration,
                              PublishYear = m.PublishYear,
                              ImdbScore = m.ImdbScore,
                              PosterUrl = m.PosterUrl,
                              TrailerUrl = m.TrailerUrl,
                              Description = m.Description,
                              GenreName = g.GenreName,
                              AgeCertificate = a.DisplayContent,
                              BackgroundColor = a.BackgroundColor,
                              ForegroundColor = a.ForegroundColor
                          }).ToList();

            var contributors = (from c in _context.Contributors
                                join p in _context.People on c.PersonId equals p.PersonId
                                join r in _context.Roles on c.RoleId equals r.RoleId
                                //where r.RoleName == "Director"
                                group p by c.MovieId into grp
                                select new
                                {
                                    MovieId = grp.Key,
                                    Contributors = string.Join(", ", grp.Select(person => person.Fullname))
                                }).ToList();

            FilmInfo = new ObservableCollection<dynamic>( (from m in movies
                            join c in contributors on m.MovieId equals c.MovieId into movieContributors
                            from mc in movieContributors.DefaultIfEmpty()
                            select new
                            {
                                m.MovieId,
                                m.Title,
                                m.Duration,
                                m.PublishYear,
                                m.ImdbScore,
                                m.PosterUrl,
                                m.TrailerUrl,
                                m.Description,
                                m.GenreName,
                                m.AgeCertificate,
                                m.BackgroundColor,
                                m.ForegroundColor,
                                Contributors = mc != null ? mc.Contributors : ""
                            }).ToList());

            var alldirectors = new ObservableCollection<dynamic>((from c in _context.Contributors
                                                                  join p in _context.People on c.PersonId equals p.PersonId
                                                                  join r in _context.Roles on c.RoleId equals r.RoleId
                                                                  where r.RoleName == "Director"
                                                                  select new
                                                                  {
                                                                      c.MovieId,
                                                                      c.PersonId,
                                                                      p.AvatarUrl,
                                                                      p.Fullname
                                                                  }).ToList());
            var allactors = new ObservableCollection<dynamic>((from c in _context.Contributors
                                                                  join p in _context.People on c.PersonId equals p.PersonId
                                                                  join r in _context.Roles on c.RoleId equals r.RoleId
                                                                  where r.RoleName == "Actor"
                                                                  select new
                                                                  {
                                                                      c.MovieId,
                                                                      c.PersonId,
                                                                      p.AvatarUrl,
                                                                      p.Fullname
                                                                  }).ToList());

            Genres = new ObservableCollection<dynamic> ((from g in _context.Genres
                                                        select new 
                                                        {
                                                           g.GenreName
                                                        }).ToList());
            foreach (var g in Genres)
            {
                string genreName = g.GenreName;
                GenresList.Add(genreName);
            }
            AgeCertificates = new ObservableCollection<dynamic>((from a in _context.AgeCertificates
                                                        select new
                                                        {
                                                            a.DisplayContent
                                                        }).ToList());
            foreach (var a in AgeCertificates)
            {
                string content = a.DisplayContent;
                AgeCertificatesList.Add(content);
            }
        }
    }
}
