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
        public ObservableCollection<dynamic> FilmInfo { get; set; } = new ObservableCollection<dynamic>();
        public ObservableCollection<dynamic> Genres { get; set; }
        public List<string> GenresList { get; set; } = new List<string>();
        public ObservableCollection<dynamic> AgeCertificates { get; set; }
        public List<string> AgeCertificatesList { get; set; } = new List<string>();
        public Admin_FilmInfo_ViewModel()
        {
            var allMovies = (from m in _context.Movies
                             join a in _context.AgeCertificates on m.AgeCertificateId equals a.AgeCertificateId
                             join g in _context.Genres on m.GenreId equals g.GenreId
                             select new
                             {
                                 MovieId = m.MovieId,
                                 Title = m.Title,
                                 Duration = m.Duration,
                                 PublishYear = m.PublishYear,
                                 ImdbScore = m.ImdbScore,
                                 AgeCertificateContent = a.DisplayContent,
                                 AgeBackground = a.BackgroundColor,
                                 AgeForeground = a.ForegroundColor,
                                 PosterUrl = m.PosterUrl,
                                 TrailerUrl = m.TrailerUrl,
                                 Description = m.Description,
                                 Genre = g.GenreName
                             }).ToList();
            foreach (var movie in allMovies)
            {
                FilmInfo.Add(new
                {
                    MovieId = movie.MovieId,
                    Title = movie.Title,
                    Duration = movie.Duration + "m",
                    PublishYear = movie.PublishYear,
                    ImdbScore = movie.ImdbScore,
                    AgeCertificateContent = movie.AgeCertificateContent,
                    PosterUrl = movie.PosterUrl,
                    TrailerUrl = movie.TrailerUrl,
                    Description = movie.Description,
                    Genre = movie.Genre,
                    AgeBackground = movie.AgeBackground,
                    AgeForeground = movie.AgeForeground
                });
            }
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
