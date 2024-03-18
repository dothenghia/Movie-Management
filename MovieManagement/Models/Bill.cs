using System;
using System.Collections.Generic;

namespace MovieManagement.Models
{
    public partial class Bill
    {
        public Bill()
        {
            BillVouchers = new HashSet<BillVoucher>();
            Tickets = new HashSet<Ticket>();
        }

        public double? Total { get; set; }

        public int? AccountId { get; set; }

        public DateTime? BookingTime { get; set; }

        public int BillId { get; set; }

        public virtual Account Account { get; set; }

        public virtual ICollection<BillVoucher> BillVouchers { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
