using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MovieManagement.ViewModels
{
    public class User_Setting_ViewModel : ViewModelBase
    {
        // Get database context
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();

        public Account UserInformation { get; set; }

        public User_Setting_ViewModel()
        {
            // Execute query to get movie Information
            UserInformation = (from a in _context.Accounts
                               where a.AccountId == 9
                               select a).FirstOrDefault();
        }

        public void UpdateFullName(string newFullName)
        {
            if (UserInformation != null)
            {
                var user = _context.Accounts.Where(a => a.AccountId == UserInformation.AccountId).FirstOrDefault();
                if (user != null)
                {
                    user.Fullname = newFullName;
                    _context.SaveChanges();
                }
            }
        }

    }
}
