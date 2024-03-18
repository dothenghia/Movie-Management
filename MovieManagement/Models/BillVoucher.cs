using System;
using System.Collections.Generic;

namespace MovieManagement.Models
{
    public partial class BillVoucher
    {
        public int BillId { get; set; }

        public int VoucherId { get; set; }

        public DateTime? AppliedTime { get; set; }

        public virtual Bill Bill { get; set; }

        public virtual Voucher Voucher { get; set; }
    }
}
