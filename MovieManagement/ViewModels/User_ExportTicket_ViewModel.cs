using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Media.MediaProperties;

namespace MovieManagement.ViewModels
{
    public class User_ExportTicket_ViewModel: ViewModelBase
    {
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public dynamic Bill { get; set; }
        public float _price;
        public float Price
        {
            get { return _price; }
            set
            {
                if (_price != value)
                {
                    _price = value; 
                    NotifyPropertyChanged(nameof(Price));
                }
            }
        }
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
            Debug.WriteLine(quantity);
            for (int j = 0; j < quantity; j++)
            {
                int i = temp.IndexOf(' ');
                string substring = temp.Substring(0, i);
                temp = temp.Replace(substring+ " ","");
                Debug.WriteLine(temp);

                foreach (var voucher_temp in AllVoucher)
                {
                    if (substring == voucher_temp.VoucherCode)
                    {
                        if (voucher_temp.IsPercentageDiscount)
                        {
                            GlobalContext.setPrice((float)(GlobalContext.price * (1- voucher_temp.DiscountAmount)));
                        }
                        else GlobalContext.setPrice((float)(GlobalContext.price - voucher_temp.DiscountAmount));
                        Debug.WriteLine(GlobalContext.price);
                    }
                }
            }
            Price = GlobalContext.price;
            //Debug.WriteLine(Price);
        }

    }

    
}
