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
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();

        public ObservableCollection<dynamic> TicketList { get; set; } = new ObservableCollection<dynamic>();

        public User_Ticket_ViewModel() 
        {
            List<string> SeatList = new List<string> { "A1", "A2" };
            var DT = DateTime.Now;
            var ShowDate = DT.ToString("ddd, dd/MMM/yyyy");
            var ShowTime = DT.ToString("HH:mm");


            // Execute query to get ticket list
            for (int i = 0; i < 10; i++)
            {
                TicketList.Add(new
                {
                    TicketId = i,
                    Title = "Movie Title", //
                    PosterUrl = "ms-appx:///Assets/thumbnail-ngang.jpg", //
                    BillId = 123, //
                    ShowDate = ShowDate,
                    ShowTime = ShowTime,
                    NumberOfTickets = 2,
                    Seat = SeatList,
                    Total = 100000
                });
            }

        }
    }
}
