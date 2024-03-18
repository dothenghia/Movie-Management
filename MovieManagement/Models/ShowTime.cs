using System;
using System.Collections.Generic;

namespace MovieManagement.Models
{
    public partial class ShowTime
    {
        public ShowTime()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int ShowTimeId { get; set; }
        public int? MovieId { get; set; }
        public DateTime? ShowDate { get; set; }
        public int? MaxRow { get; set; }
        public int? MaxCol { get; set; }

        public virtual Movie? Movie { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
