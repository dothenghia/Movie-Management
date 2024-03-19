using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MovieManagement.ViewModels
{
    public class User_Movie_ViewModel : ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();

        // 
        public dynamic MovieInformation { get; set; }
        public ObservableCollection<dynamic> MovieDirectors { get; set; }
        public ObservableCollection<dynamic> MovieActors { get; set; }

        public User_Movie_ViewModel(int MovieID) 
        {
            // Execute query to get movie Information
            MovieInformation = (from m in _context.Movies
                                join a in _context.AgeCertificates on m.AgeCertificateId equals a.AgeCertificateId
                                join g in _context.Genres on m.GenreId equals g.GenreId
                                where m.MovieId == MovieID
                                select new
                                {
                                    m.MovieId,
                                    m.Title,
                                    m.Duration,
                                    m.PublishYear,
                                    m.ImdbScore,
                                    a.DisplayContent,
                                    a.BackgroundColor,
                                    a.ForegroundColor,
                                    m.PosterUrl,
                                    m.TrailerUrl,
                                    m.Description,
                                    g.GenreName
                                }).FirstOrDefault();

            MovieDirectors = new ObservableCollection<dynamic>((from c in _context.Contributors
                                                                join p in _context.People on c.PersonId equals p.PersonId
                                                                join r in _context.Roles on c.RoleId equals r.RoleId
                                                                where c.MovieId == MovieID && r.RoleName == "Director"
                                                                select new
                                                                {
                                                                    p.Fullname,
                                                                    p.AvatarUrl,
                                                                    p.Biography,
                                                                }).ToList());

            MovieActors = new ObservableCollection<dynamic>((from c in _context.Contributors
                                                             join p in _context.People on c.PersonId equals p.PersonId
                                                             join r in _context.Roles on c.RoleId equals r.RoleId
                                                             join m in _context.Movies on c.MovieId equals m.MovieId
                                                             where c.MovieId == MovieID && r.RoleName == "Actor"
                                                             select new
                                                             {
                                                                 p.Fullname,
                                                                 p.AvatarUrl,
                                                                 p.Biography,
                                                             }).ToList());

        }

    }
}
