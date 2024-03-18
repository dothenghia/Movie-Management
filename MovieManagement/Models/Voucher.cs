using System;
using System.Collections.Generic;

namespace MovieManagement.Models
{
    public partial class Voucher
    {
        public Voucher()
        {
            BillVouchers = new HashSet<BillVoucher>();
        }

        public string? VoucherCode { get; set; }
        public double? DiscountAmount { get; set; }
        public bool? IsExpired { get; set; }
        public bool? IsPercentageDiscount { get; set; }
        public double? RequirementAmount { get; set; }
        public int VoucherId { get; set; }

        public virtual ICollection<BillVoucher> BillVouchers { get; set; }
    }
}
