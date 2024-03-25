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
                                         group ticket by new { bill.BillId, movie.Title, movie.PosterUrl, showTime.ShowDate, bill.Total, bill.BookingTime } into g
                                         select new
                                         {
                                             PosterUrl = g.Key.PosterUrl,
                                             Title = g.Key.Title,
                                             BillId = g.Key.BillId,
                                             ShowDate = g.Key.ShowDate,
                                             NumberOfTickets = g.Count(),
                                             Total = g.Key.Total,
                                             Seat = string.Join(", ", g.Select(t => t.Row + t.Col)),
                                             Price = g.Select(t => t.Price).FirstOrDefault(),
                                             Voucher = (from bv in _context.BillVouchers
                                                        join v in _context.Vouchers on bv.VoucherId equals v.VoucherId
                                                        where bv.BillId == billID
                                                        select v.VoucherCode).ToList(),
                                             BookTime = g.Key.BookingTime,
                                         }).FirstOrDefault();

            if (TempTicketInformation != null)
            {
                var DT = (DateTime)TempTicketInformation.ShowDate;
                var SD = DT.ToString("ddd, dd/MMM/yyyy");
                var ST = DT.ToString("HH:mm");

                var BT = (DateTime)TempTicketInformation.BookTime;
                var BTString = BT.ToString("ddd, dd/MMM/yyyy HH:mm");

                if (TempTicketInformation.Voucher.Count == 0) {
                    TempTicketInformation.Voucher.Add("---");
                }
                TicketInformation = new
                {
                    PosterUrl = TempTicketInformation.PosterUrl,
                    Title = TempTicketInformation.Title,
                    BillId = TempTicketInformation.BillId,
                    ShowTime = ST,
                    ShowDate = SD,
                    NumberOfTickets = TempTicketInformation.NumberOfTickets,
                    Total = TempTicketInformation.Total.ToString(),
                    SeatList = GetSeatList(TempTicketInformation.Seat),
                    Price = TempTicketInformation.Price.ToString(),
                    RawPrice = (TempTicketInformation.Price * TempTicketInformation.NumberOfTickets).ToString(),
                    DiscountPrice = (TempTicketInformation.Total - (TempTicketInformation.Price * TempTicketInformation.NumberOfTickets)).ToString(),
                    BookTime = BTString,
                    VoucherList = TempTicketInformation.Voucher
                };
            }
        }

        private List<string> GetSeatList(string seatString)
        {
            return seatString.Split(", ").ToList();
        }
    }
}
