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
        public DateTime DateOfBirth { get; set; }

        public User_Setting_ViewModel()
        {
            // Execute query to get movie Information
            UserInformation = (from a in _context.Accounts
                               where a.AccountId == GlobalContext.UserID
                               select a).FirstOrDefault();
            DateOfBirth = (DateTime)UserInformation.Dob;
            DateOfBirth = DateOfBirth.Date;
        }

        // -- Update Fullname
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

        // -- Check Password
        public bool CheckPassword(string hashedPassword)
        {
            if (UserInformation != null)
            {
                var user = _context.Accounts.Where(a => a.AccountId == UserInformation.AccountId).FirstOrDefault();
                if (user != null)
                {
                    if (user.Password == hashedPassword)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // -- Update Password
        public void UpdatePassword(string newPassword)
        {
            if (UserInformation != null)
            {
                var user = _context.Accounts.Where(a => a.AccountId == UserInformation.AccountId).FirstOrDefault();
                if (user != null)
                {
                    user.Password = newPassword;
                    _context.SaveChanges();
                }
            }
        }

    }
}
