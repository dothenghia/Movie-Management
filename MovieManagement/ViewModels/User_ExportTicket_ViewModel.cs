using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
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
        public RelayCommand ConfirmCommand { get; }
        public User_ExportTicket_ViewModel()
        {
            ApplyCommand = new RelayCommand(ApplyVoucher);
            ConfirmCommand = new RelayCommand(Confirm);
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
            
            
            AllVoucher = new ObservableCollection<dynamic>((from v in _context.Vouchers
                          where v.IsExpired == false
                          select new
                          {
                              v.VoucherCode,
                              isAvailable = true,
                              v.DiscountAmount,
                              v.IsPercentageDiscount
                          }).ToList());

            var AllVoucherIncludeExpired = (from v in _context.Vouchers
                                            select new
                                            {
                                                v.VoucherCode,
                                                v.DiscountAmount,
                                                v.IsPercentageDiscount,
                                            }).ToList();
            if (quantity > 0)
            {
                GlobalContext.setPrice((float)(Bill.Price * quantity));
                DateTime thisMonth = DateTime.Now;
                int month = thisMonth.Month;
                var dob = (from a in _context.Accounts
                           where a.AccountId == GlobalContext.UserID
                           select new
                           {
                               Month = a.Dob.Value.Month
                           }).FirstOrDefault();
                if (dob.Month == month)
                {
                    foreach (var voucher_temp in AllVoucherIncludeExpired)
                    {
                        Debug.WriteLine("haah");
                        if (voucher_temp.VoucherCode == "Birthday")
                        {
                            if (voucher_temp.IsPercentageDiscount == true)
                            {
                                GlobalContext.setPrice((float)(GlobalContext.price * (1 - voucher_temp.DiscountAmount)));
                            }
                            else GlobalContext.setPrice((float)(GlobalContext.price - voucher_temp.DiscountAmount));
                            Debug.WriteLine("hmmmm");
                        }
                    }
                }
                Price = GlobalContext.price;
            }
        }

        private void ApplyVoucher()
        {
            
            string temp = GlobalContext.voucher;
            int quantity = temp.Count(c => c == ' ');
            for (int j = 0; j < quantity; j++)
            {
                int i = temp.IndexOf(' ');
                string substring = temp.Substring(0, i);
                temp = temp.Replace(substring+ " ","");

                foreach (var voucher_temp in AllVoucher)
                {
                    if (substring == voucher_temp.VoucherCode)
                    {
                        if (voucher_temp.IsPercentageDiscount)
                        {
                            GlobalContext.setPrice((float)(GlobalContext.price * (1- voucher_temp.DiscountAmount)));
                        }
                        else GlobalContext.setPrice((float)(GlobalContext.price - voucher_temp.DiscountAmount));
                    }
                }
            }
            Price = GlobalContext.price;
        }

        private void Confirm()
        {
            int newBillID;
            var newBill = new Bill { 
                AccountId = GlobalContext.UserID,
                Total = GlobalContext.price,
                BookingTime = DateTime.Now
            };
            _context.Bills.Add(newBill);
            _context.SaveChanges();
            newBillID = newBill.BillId;

            while (GlobalContext.seats != "")
            {
                int i = GlobalContext.seats.IndexOf(' ');
                string substring = GlobalContext.seats.Substring(0, i);
                GlobalContext.removeSeat(substring);
                var updateTicket = _context.Tickets.FirstOrDefault(t => t.Row+ t.Col.ToString() == substring);
                if (updateTicket != null)
                {
                    updateTicket.BillId = newBillID;
                    updateTicket.IsAvailable = false;
                    _context.SaveChanges();
                }
            }

            while (GlobalContext.voucher != "")
            {
                int i = GlobalContext.voucher.IndexOf(' ');
                string substring = GlobalContext.voucher.Substring(0, i);
                GlobalContext.voucher = GlobalContext.voucher.Replace(substring + " ", "");
                var voucherID = _context.Vouchers.FirstOrDefault(v => v.VoucherCode == substring);
                var newBillVoucher = new BillVoucher
                {
                    BillId = newBillID,
                    VoucherId = voucherID.VoucherId,
                    AppliedTime = DateTime.Now,
                };
                _context.BillVouchers.Add(newBillVoucher);
                _context.SaveChanges();
            }
        }

    }

    
}
