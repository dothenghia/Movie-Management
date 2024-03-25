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
            _moviesCount = (from m in _context.Movies
                            join s in _context.ShowTimes on m.MovieId equals s.MovieId
                            select m).Distinct().Count().ToString();

            DateTime currentDate = DateTime.Now.Date;
            _showtimesDaily = _context.ShowTimes
                .Where(st => st.ShowDate.Value.Date == currentDate)
                .Count().ToString();

            DateTime startOfWeek = currentDate.Date.AddDays(-(int)currentDate.DayOfWeek).Date;
            DateTime endOfWeek = startOfWeek.AddDays(6).Date;
            Debug.Print(startOfWeek.ToString() + "   " + currentDate.DayOfWeek.ToString());
            _showtimesWeekly = _context.ShowTimes
                .Where(st => st.ShowDate.Value.Date >= startOfWeek && st.ShowDate.Value.Date <= endOfWeek)
                .Count().ToString();

            DateTime startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1).Date;
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1).Date;
            _showtimesMonthly = _context.ShowTimes
                .Where(st => st.ShowDate.Value.Date >= startOfMonth && st.ShowDate <= endOfMonth)
                .Count().ToString();
        }
    }
}
