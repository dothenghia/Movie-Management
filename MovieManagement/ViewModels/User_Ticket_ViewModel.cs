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
    public class User_Ticket_ViewModel : ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();

        public ObservableCollection<dynamic> TicketList { get; set; } = new ObservableCollection<dynamic>();


        public User_Ticket_ViewModel()
        {
            LoadTicketList();
        }


        private void LoadTicketList()
        {
            var tempTickets = (from bill in _context.Bills
                           join ticket in _context.Tickets on bill.BillId equals ticket.BillId
                           join showTime in _context.ShowTimes on ticket.ShowTimeId equals showTime.ShowTimeId
                           join movie in _context.Movies on showTime.MovieId equals movie.MovieId
                           where bill.AccountId == GlobalContext.UserID
                           group ticket by new { bill.BillId, movie.Title, movie.PosterUrl, showTime.ShowDate, bill.Total } into g
                           select new
                           {
                               Title = g.Key.Title,
                               PosterUrl = g.Key.PosterUrl,
                               BillId = g.Key.BillId,
                               ShowDate = g.Key.ShowDate,
                               NumberOfTickets = g.Count(),
                               Total = g.Key.Total
                           }).ToList();

            foreach (var ticket in tempTickets)
            {
                var DT = (DateTime)ticket.ShowDate;
                var SD = DT.ToString("ddd, dd/MMM/yyyy");
                var ST = DT.ToString("HH:mm");
                var item = new
                {
                    Title = ticket.Title,
                    PosterUrl = ticket.PosterUrl,
                    BillId = ticket.BillId,
                    ShowDate = SD,
                    ShowTime = ST,
                    NumberOfTickets = ticket.NumberOfTickets,
                    Total = ticket.Total.ToString()
                };
                TicketList.Add(item);
            }
        }
    }
}
