using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MovieManagement.ViewModels
{
    public class Admin_ShowTime_ViewModel : ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public ObservableCollection<dynamic> Showtimes { get; set; } = new ObservableCollection<dynamic>();
        public Admin_ShowTime_ViewModel()
        {
            var allshowtimes = new ObservableCollection<dynamic>((from s in _context.ShowTimes
                                                           join m in _context.Movies on s.MovieId equals m.MovieId
                                                           where s.ShowDate >= DateTime.Now.Date
                                                            select new
                                                            {
                                                                s.ShowTimeId,
                                                                s.ShowDate,
                                                                s.MovieId,
                                                                m.PosterUrl,
                                                                m.Title,
                                                                s.MaxRow,
                                                                s.MaxCol
                                                            }).ToList());
            foreach(var s in allshowtimes)
            {
                bool IsEditButtonEnabled = false;
                bool IsDeleteButtonEnabled = false;
                if (s.ShowDate >= DateTime.Now.Date.AddDays(7)) { IsEditButtonEnabled = true; IsDeleteButtonEnabled = false; }
                Showtimes.Add(new
                {
                    s.ShowTimeId,
                    s.ShowDate,
                    s.MovieId,
                    s.PosterUrl,
                    s.Title,
                    s.MaxRow,
                    s.MaxCol,
                    Date = s.ShowDate.Day + "/" + s.ShowDate.Month + "/" + s.ShowDate.Year,
                    Time = s.ShowDate.TimeOfDay,
                    IsEditButtonEnabled = IsEditButtonEnabled,
                    IsDeleteButtonEnabled = IsDeleteButtonEnabled,
                });
            }
        }
    }
}
