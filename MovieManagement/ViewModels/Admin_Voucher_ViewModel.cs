﻿using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MovieManagement.ViewModels
{
    public class Admin_Voucher_ViewModel : ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public ObservableCollection<dynamic> Vouchers { get; set; }
        public Admin_Voucher_ViewModel() 
        {
            Vouchers = new ObservableCollection<dynamic>();
        }


        public void Update_Voucher()
        {
            Vouchers.Clear();
            var allVouchers = (from v in _context.Vouchers
                               select new
                               {
                                   v.VoucherCode,
                                   v.VoucherId,
                                   v.DiscountAmount,
                                   v.IsExpired,
                                   v.IsPercentageDiscount,
                                   v.RequirementAmount,
                               }
                               ).ToList();
            foreach (var voucher in allVouchers)
            {
                Vouchers.Add( voucher );
            }
        }}
}
