using MovieManagement.Models;
using MovieManagement.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManagement.ViewModels
{
    public class User_DetailTicket_ViewModel : ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();

        public dynamic TicketInformation { get; set; }

        public User_DetailTicket_ViewModel(int billID)
        {
            LoadTicketDetail(billID);
        }

        public void LoadTicketDetail(int billID)
        {
            var TempTicketInformation = (from bill in _context.Bills
                                         join ticket in _context.Tickets on bill.BillId equals ticket.BillId
                                         join showTime in _context.ShowTimes on ticket.ShowTimeId equals showTime.ShowTimeId
                                         join movie in _context.Movies on showTime.MovieId equals movie.MovieId
                                         where bill.BillId == billID
                                         group ticket by new { bill.BillId, movie.Title, movie.PosterUrl, showTime.ShowDate, bill.Total } into g
                                         select new
                                         {
                                             Title = g.Key.Title,
                                             PosterUrl = g.Key.PosterUrl,
                                             BillId = g.Key.BillId,
                                             ShowDate = g.Key.ShowDate,
                                             NumberOfTickets = g.Count(),
                                             Seat = string.Join(", ", g.Select(t => t.Row + t.Col)),
                                             Total = g.Key.Total
                                         }).FirstOrDefault();
            if (TempTicketInformation != null)
            {
                var DT = (DateTime)TempTicketInformation.ShowDate;
                var SD = DT.ToString("ddd, dd/MMM/yyyy");
                var ST = DT.ToString("HH:mm");
                TicketInformation = new
                {
                    Title = TempTicketInformation.Title,
                    PosterUrl = TempTicketInformation.PosterUrl,
                    BillId = TempTicketInformation.BillId,
                    ShowDate = SD,
                    ShowTime = ST,
                    NumberOfTickets = TempTicketInformation.NumberOfTickets,
                    Seat = TempTicketInformation.Seat,
                    Total = TempTicketInformation.Total
                };
            }
        }
    }
}
