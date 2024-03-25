using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace MovieManagement.ViewModels
{
    public class User_Search_ViewModel : ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();
        private const int PageSize = 3;
        private int _currentPage = 1;
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                NotifyPropertyChanged(nameof(CurrentPage));
            }
        }
        public ICommand NextPageCommand { get; set; }
        public ICommand PreviousPageCommand { get; set; }
        public ObservableCollection<dynamic> Movies { get; set; }
        private ObservableCollection<dynamic> _pagedMovies;
        public ObservableCollection<dynamic> PagedMovies
        {
            get => _pagedMovies;
            set
            {
                _pagedMovies = value;
                NotifyPropertyChanged(nameof(PagedMovies));
            }
        }
        private int _totalPages;
        public int TotalPages
        {
            get { return _totalPages; }
            set
            {
                _totalPages = value;
                NotifyPropertyChanged(nameof(TotalPages));
            }
        }
        public ObservableCollection<dynamic> MoviestoRemove {  get; set; }
        public string stored_sort="";
        public string stored_genre = "";
        public string stored_year = "";
        public string stored_agecertification = "";
        public User_Search_ViewModel()
        {
            Movies = new ObservableCollection<dynamic>();
            MoviestoRemove = new ObservableCollection<dynamic>();
            PagedMovies = new ObservableCollection<dynamic>();
            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
        }

        public void Update_MovieSort(string filter)
        {
            Movies.Clear();
            stored_sort = filter;
            var movie_from_movie_name = (from m in _context.Movies
                                         join a in _context.AgeCertificates on m.AgeCertificateId equals a.AgeCertificateId
                                         join g in _context.Genres on m.GenreId equals g.GenreId
                                         where m.Title == GlobalContext.name
                                         select new
                                         {
                                             m.PosterUrl,
                                             m.Duration,
                                             m.Title,
                                             m.ImdbScore,
                                             m.PublishYear,
                                             a.DisplayContent,
                                             g.GenreName,
                                         }).FirstOrDefault();
            if (movie_from_movie_name != null)
            {
                Movies.Add(movie_from_movie_name);
                return;
            }
               
            else
            {
                if (filter == "Publish year")
                {
                    var all_movies_from_star_name = (from m in _context.Movies
                                                     join c in _context.Contributors on m.MovieId equals c.MovieId
                                                     join p in _context.People on c.PersonId equals p.PersonId
                                                     join a in _context.AgeCertificates on m.AgeCertificateId equals a.AgeCertificateId
                                                     join g in _context.Genres on m.GenreId equals g.GenreId
                                                     where p.Fullname == GlobalContext.name
                                                     select new
                                                     {
                                                         m.PosterUrl,
                                                         m.Duration,
                                                         m.Title,
                                                         a.DisplayContent,
                                                         m.PublishYear,
                                                         m.ImdbScore,
                                                         g.GenreName,
                                                     }).OrderByDescending(m => m.PublishYear).ToList();

                        foreach (var movies in all_movies_from_star_name)
                        {
                            Movies.Add(movies);
                        }
                }
                if (filter == "IMDB score")
                {
                    var all_movies_from_star_name = (from m in _context.Movies
                                                     join c in _context.Contributors on m.MovieId equals c.MovieId
                                                     join p in _context.People on c.PersonId equals p.PersonId
                                                     join a in _context.AgeCertificates on m.AgeCertificateId equals a.AgeCertificateId
                                                     join g in _context.Genres on m.GenreId equals g.GenreId
                                                     where p.Fullname == GlobalContext.name
                                                     select new
                                                     {
                                                         m.PosterUrl,
                                                         m.Duration,
                                                         m.Title,
                                                         a.DisplayContent,
                                                         m.PublishYear,
                                                         m.ImdbScore,
                                                         g.GenreName
                                                     }).OrderByDescending(m => m.ImdbScore).ToList();

                        foreach (var movies in all_movies_from_star_name)
                        {
                            Movies.Add(movies);
                        }

                }
                if (filter == "Duration")
                {
                    var all_movies_from_star_name = (from m in _context.Movies
                                                     join c in _context.Contributors on m.MovieId equals c.MovieId
                                                     join p in _context.People on c.PersonId equals p.PersonId
                                                     join a in _context.AgeCertificates on m.AgeCertificateId equals a.AgeCertificateId
                                                     join g in _context.Genres on m.GenreId equals g.GenreId
                                                     where p.Fullname == GlobalContext.name
                                                     select new
                                                     {
                                                         m.PosterUrl,
                                                         m.Duration,
                                                         m.Title,
                                                         a.DisplayContent,
                                                         m.PublishYear,
                                                         m.ImdbScore,
                                                         g.GenreName
                                                     }).OrderByDescending(m => m.Duration).ToList();

                        foreach (var movies in all_movies_from_star_name)
                        {
                            Movies.Add(movies);
                        }
                }
            }

            UpdatePagedMovies();
        }
    
        public void UpdatePagedMovies()
        {
            TotalPages = (int)Math.Ceiling((double)Movies.Count / PageSize);
            PagedMovies = new ObservableCollection<dynamic>(Movies.Skip((_currentPage - 1) * PageSize).Take(PageSize));
        }

        public void NextPage()
        {
            if(_currentPage < TotalPages)
            {
                _currentPage++;
                UpdatePagedMovies();
            }
        }

        public void PreviousPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                UpdatePagedMovies();
            }
        }

        public void Update_Movie()
        {
            Movies.Clear();
            var movie_from_movie_name = (from m in _context.Movies
                                         join a in _context.AgeCertificates on m.AgeCertificateId equals a.AgeCertificateId
                                         join g in _context.Genres on m.GenreId equals g.GenreId
                                         where m.Title == GlobalContext.name
                                         select new
                                         {
                                             m.PosterUrl,
                                             m.Duration,
                                             m.Title,
                                             m.ImdbScore,
                                             m.PublishYear,
                                             a.DisplayContent,
                                             g.GenreName,
                                         }).FirstOrDefault();
            if (movie_from_movie_name != null)
            {
                if (stored_agecertification == stored_genre && stored_genre == stored_year)
                    Movies.Add(movie_from_movie_name);
                if (stored_year == stored_genre && movie_from_movie_name.DisplayContent == stored_agecertification)
                    Movies.Add(movie_from_movie_name);
                if (stored_year == stored_agecertification && movie_from_movie_name.GenreName == stored_genre)
                    Movies.Add(movie_from_movie_name);
                if (stored_genre == stored_agecertification && movie_from_movie_name.PublishYear.ToString() == stored_year)
                    Movies.Add(movie_from_movie_name);
                if (movie_from_movie_name.GenreName == stored_genre && movie_from_movie_name.PublishYear.ToString() == stored_year && (stored_agecertification == "" || stored_agecertification=="All"))
                    Movies.Add(movie_from_movie_name);
                if (movie_from_movie_name.GenreName == stored_genre && movie_from_movie_name.DisplayContent == stored_agecertification && (stored_year == ""|| stored_year == "All"))
                    Movies.Add(movie_from_movie_name);  
                if (movie_from_movie_name.PublishYear.ToString() == stored_year &&  movie_from_movie_name.DisplayContent == stored_agecertification && (stored_genre == "" || stored_genre == "All"))
                    Movies.Add(movie_from_movie_name);
                if (movie_from_movie_name.PublishYear.ToString() == stored_year &&  movie_from_movie_name.DisplayContent == stored_agecertification &&   movie_from_movie_name.GenreName == stored_genre)
                    Movies.Add(movie_from_movie_name);
                return;
            }

            else
            {
                if (stored_sort == "Publish year")
                {
                    var all_movies_from_star_name = (from m in _context.Movies
                                                     join c in _context.Contributors on m.MovieId equals c.MovieId
                                                     join p in _context.People on c.PersonId equals p.PersonId
                                                     join a in _context.AgeCertificates on m.AgeCertificateId equals a.AgeCertificateId
                                                     join g in _context.Genres on m.GenreId equals g.GenreId
                                                     where p.Fullname == GlobalContext.name
                                                     select new
                                                     {
                                                         m.PosterUrl,
                                                         m.Duration,
                                                         m.Title,
                                                         a.DisplayContent,
                                                         m.PublishYear,
                                                         m.ImdbScore,
                                                         g.GenreName,
                                                     }).OrderByDescending(m => m.PublishYear).ToList();

                    foreach (var movies in all_movies_from_star_name)
                    {
                        if (stored_agecertification == stored_genre && stored_genre == stored_year)
                            Movies.Add(movies);
                        if (stored_year == stored_genre && movies.DisplayContent == stored_agecertification)
                            Movies.Add(movies);
                        if (stored_year == stored_agecertification && movies.GenreName == stored_genre) 
                            Movies.Add(movies);
                        if (stored_genre ==  stored_agecertification && movies.PublishYear.ToString() == stored_year)
                            Movies.Add(movies);
                        if (movies.GenreName == stored_genre && movies.PublishYear.ToString() == stored_year && stored_agecertification == "")
                            Movies.Add(movies);
                        if (movies.GenreName == stored_genre && movies.DisplayContent == stored_agecertification && stored_year == "")
                            Movies.Add(movies);
                        if (movies.PublishYear.ToString() == stored_year && movies.DisplayContent == stored_agecertification && stored_genre=="")
                            Movies.Add(movies);
                        if (movies.PublishYear.ToString() == stored_year && movies.DisplayContent == stored_agecertification && movies.GenreName == stored_genre)
                            Movies.Add(movies);
                    }
                }
                if (stored_sort == "IMDB score")
                {
                    var all_movies_from_star_name = (from m in _context.Movies
                                                     join c in _context.Contributors on m.MovieId equals c.MovieId
                                                     join p in _context.People on c.PersonId equals p.PersonId
                                                     join a in _context.AgeCertificates on m.AgeCertificateId equals a.AgeCertificateId
                                                     join g in _context.Genres on m.GenreId equals g.GenreId
                                                     where p.Fullname == GlobalContext.name
                                                     select new
                                                     {
                                                         m.PosterUrl,
                                                         m.Duration,
                                                         m.Title,
                                                         a.DisplayContent,
                                                         m.PublishYear,
                                                         m.ImdbScore,
                                                         g.GenreName
                                                     }).OrderByDescending(m => m.ImdbScore).ToList();

                    foreach (var movies in all_movies_from_star_name)
                    {
                        if (stored_agecertification == stored_genre && stored_genre == stored_year)
                            Movies.Add(movies);
                        if (stored_year == stored_genre && movies.DisplayContent == stored_agecertification)
                            Movies.Add(movies);
                        if (stored_year == stored_agecertification && movies.GenreName == stored_genre)
                            Movies.Add(movies);
                        if (stored_genre == stored_agecertification && movies.PublishYear.ToString() == stored_year)
                            Movies.Add(movies);
                        if (movies.GenreName == stored_genre && movies.PublishYear.ToString() == stored_year && stored_agecertification == "")
                            Movies.Add(movies);
                        if (movies.GenreName == stored_genre && movies.DisplayContent == stored_agecertification && stored_year == "")
                            Movies.Add(movies);
                        if (movies.PublishYear.ToString() == stored_year && movies.DisplayContent == stored_agecertification && stored_genre == "")
                            Movies.Add(movies);
                        if (movies.PublishYear.ToString() == stored_year && movies.DisplayContent == stored_agecertification && movies.GenreName == stored_genre)
                            Movies.Add(movies);
                    }

                }
                if (stored_sort == "Duration")
                {
                    var all_movies_from_star_name = (from m in _context.Movies
                                                     join c in _context.Contributors on m.MovieId equals c.MovieId
                                                     join p in _context.People on c.PersonId equals p.PersonId
                                                     join a in _context.AgeCertificates on m.AgeCertificateId equals a.AgeCertificateId
                                                     join g in _context.Genres on m.GenreId equals g.GenreId
                                                     where p.Fullname == GlobalContext.name
                                                     select new
                                                     {
                                                         m.PosterUrl,
                                                         m.Duration,
                                                         m.Title,
                                                         a.DisplayContent,
                                                         m.PublishYear,
                                                         m.ImdbScore,
                                                         g.GenreName
                                                     }).OrderByDescending(m => m.Duration).ToList();

                    foreach (var movies in all_movies_from_star_name)
                    {
                        if (stored_agecertification == stored_genre && stored_genre == stored_year)
                            Movies.Add(movies);
                        if (stored_year == stored_genre && movies.DisplayContent == stored_agecertification)
                            Movies.Add(movies);
                        if (stored_year == stored_agecertification && movies.GenreName == stored_genre)
                            Movies.Add(movies);
                        if (stored_genre == stored_agecertification && movies.PublishYear.ToString() == stored_year)
                            Movies.Add(movies);
                        if (movies.GenreName == stored_genre && movies.PublishYear.ToString() == stored_year && stored_agecertification == "")
                            Movies.Add(movies);
                        if (movies.GenreName == stored_genre && movies.DisplayContent == stored_agecertification && stored_year == "")
                            Movies.Add(movies);
                        if (movies.PublishYear.ToString() == stored_year && movies.DisplayContent == stored_agecertification && stored_genre == "")
                            Movies.Add(movies);
                        if (movies.PublishYear.ToString() == stored_year && movies.DisplayContent == stored_agecertification && movies.GenreName == stored_genre)
                            Movies.Add(movies);
                    }
                }
            }

            UpdatePagedMovies();
        }
    }
}
