using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.MediaProperties;

namespace MovieManagement.ViewModels
{
    public class User_ExportTicket_ViewModel:ViewModelBase
    {
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public dynamic Bill { get; set; }
        public float Price { get; set; }
        public ObservableCollection<dynamic> AllVoucher { get; set; }
        public RelayCommand ApplyCommand { get; }
        public User_ExportTicket_ViewModel()
        {
            ApplyCommand = new RelayCommand(ApplyVoucher);
            var temp = GlobalContext.seats;
            int quantity = temp.Count(c => c == ' ');
            Bill = (from s in _context.ShowTimes
                    join m in _context.Movies on s.MovieId equals m.MovieId
                    join t in _context.Tickets on s.ShowTimeId equals t.ShowTimeId
                    where s.ShowTimeId == GlobalContext.showtimeID
                    select new
                    {
                        m.Title,
                        Quantity = quantity,
                        s.ShowDate,
                        Seat = temp,
                        t.Price
                    }).FirstOrDefault();
            if (quantity > 0)
            {
                GlobalContext.setPrice((float)(Bill.Price * quantity));
                Price = GlobalContext.price;
            }
            AllVoucher = new ObservableCollection<dynamic>((from v in _context.Vouchers
                          where v.IsExpired == false
                          select new
                          {
                              v.VoucherCode,
                              isAvailable = true,
                              v.DiscountAmount,
                              v.IsPercentageDiscount
                          }).ToList());
        }

        private void ApplyVoucher()
        {
            string temp = GlobalContext.voucher;
            int quantity = temp.Count(c => c == ' ');
            for (int j = 0; j < quantity-1; j++)
            {
                int i = temp.IndexOf(' ');
                string substring = temp.Substring(0, i);
                if (temp.Contains(substring)) {
                    temp = temp.Replace(substring,"");
                }
                foreach (var voucher_temp in AllVoucher)
                {
                    if (substring == voucher_temp.VoucherCode)
                        if (voucher_temp.IsPercentageDiscount)
                        {
                            GlobalContext.setPrice((float)(GlobalContext.price * voucher_temp.DiscountAmount));
                        }
                        else GlobalContext.setPrice((float)(GlobalContext.price - voucher_temp.DiscountAmount));
                }
            }
            Price = GlobalContext.price;
        }

    }

    
}
