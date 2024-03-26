using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MovieManagement.Views;

namespace MovieManagement.ViewModels
{
    public class Admin_Tickets_ViewModel : ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public ObservableCollection<dynamic> Tickets { get; set; }
        public Admin_Tickets_ViewModel() 
        {
            Tickets = new ObservableCollection<dynamic>();
        }
        public void Update_Tickets()
        {
            Tickets.Clear();
            var alltickets = new ObservableCollection<dynamic>((from t in _context.Tickets
                                                                join s in _context.ShowTimes on t.ShowTimeId equals s.ShowTimeId
                                                                join m in _context.Movies on s.MovieId equals m.MovieId
                                                                where s.ShowDate >= DateTime.Now.Date
                                                                orderby s.ShowDate descending
                                                                select new
                                                                {
                                                                    t.TicketId,
                                                                    t.Row,
                                                                    t.Col,
                                                                    t.Price,
                                                                    t.IsAvailable,
                                                                    s.ShowTimeId,
                                                                    s.ShowDate,
                                                                    m.Title,
                                                                    m.PosterUrl
                                                                }).ToList());
            foreach (var t in alltickets)
            {
                Tickets.Add(new
                {
                    t.TicketId,
                    t.Row,
                    t.Col,
                    t.Price,
                    t.IsAvailable,
                    t.ShowTimeId,
                    t.ShowDate,
                    t.Title,
                    t.PosterUrl,
                    IsButtonEnabled = t.IsAvailable,
                });
            }
        }
    }
}
