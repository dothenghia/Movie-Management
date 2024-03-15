using System;
using System.Collections.Generic;

namespace MovieManagement.Models
{
    public partial class Account
    {
        public Account()
        {
            Bills = new HashSet<Bill>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public string Fullname { get; set; }
        public bool? IsAdmin { get; set; }
        public int AccountId { get; set; }

        public virtual ICollection<Bill> Bills { get; set; }
    }
}
