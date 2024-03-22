using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;

namespace MovieManagement.ViewModels
{
    public class Admin_Main_ViewModel : ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();

        public ObservableCollection<dynamic> Movies { get; set; }
        public ObservableCollection<dynamic> Dashboard { get; set; }
        string _moviesCount;
        public string moviesCount
        {
            get { return _moviesCount; }
            set { _moviesCount = value; NotifyPropertyChanged(nameof(moviesCount)); }
        }
        string _showtimesDaily;
        public string showtimesDaily
        {
            get { return _showtimesDaily; }
            set { _showtimesDaily = value; NotifyPropertyChanged(nameof(showtimesDaily)); }
        }
        string _showtimesWeekly;
        public string showtimesWeekly
        {
            get { return _showtimesWeekly; }
            set { _showtimesWeekly = value; NotifyPropertyChanged(nameof(showtimesWeekly)); }
        }
        string _showtimesMonthly;
        public string showtimesMonthly
        {
            get { return _showtimesMonthly; }
            set { _showtimesMonthly = value; NotifyPropertyChanged(nameof(showtimesMonthly)); }
        }
        public Admin_Main_ViewModel() 
        {
            Movies = new ObservableCollection<dynamic>((from m in _context.Movies
                                                        join a in _context.AgeCertificates on m.AgeCertificateId equals a.AgeCertificateId
                                                        join g in _context.Genres on m.GenreId equals g.GenreId
                                                        where m.IsBlockbuster == true
                                                        select new
                                                        {
                                                            m.MovieId,
                                                            m.Title,
                                                            m.ImdbScore,
                                                            a.DisplayContent,
                                                            g.GenreName,
                                                            m.PosterUrl,
                                                        }).ToList());
            _moviesCount = _context.Movies.Count().ToString();

            DateTime currentDate = DateTime.Now;
            _showtimesDaily = _context.ShowTimes
                .Where(st => st.ShowDate == currentDate)
                .Count().ToString();

            DateTime startOfWeek = currentDate.Date.AddDays(-(int)currentDate.DayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            _showtimesWeekly = _context.ShowTimes
                .Where(st => st.ShowDate >= startOfWeek && st.ShowDate <= endOfWeek)
                .Count().ToString();

            DateTime startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            _showtimesMonthly = _context.ShowTimes
                .Where(st => st.ShowDate >= startOfMonth && st.ShowDate <= endOfMonth)
                .Count().ToString();
        }
    }
}
