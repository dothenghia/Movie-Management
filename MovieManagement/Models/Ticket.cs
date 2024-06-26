﻿using System;
using System.Collections.Generic;

namespace MovieManagement.Models
{
    public partial class Ticket
    {
        public int TicketId { get; set; }
        public bool? IsAvailable { get; set; }
        public string? Row { get; set; }
        public int? Col { get; set; }
        public double? Price { get; set; }
        public int? BillId { get; set; }
        public int? ShowTimeId { get; set; }

        public virtual Bill? Bill { get; set; }
        public virtual ShowTime? ShowTime { get; set; }
    }
}
