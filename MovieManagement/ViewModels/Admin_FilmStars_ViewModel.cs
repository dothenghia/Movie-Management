using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MovieManagement.ViewModels
{
    public class Admin_FilmStars_ViewModel : ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public ObservableCollection<dynamic> MovieStars { get; set; }

        public Admin_FilmStars_ViewModel() 
        {
            MovieStars = new ObservableCollection<dynamic>((from c in _context.Contributors
                                                                join p in _context.People on c.PersonId equals p.PersonId
                                                                join r in _context.Roles on c.RoleId equals r.RoleId
                                                                where r.RoleName == "Actor"
                                                                select new
                                                                {
                                                                    p.Fullname,
                                                                    p.AvatarUrl,
                                                                    p.Biography,
                                                                }).Distinct().ToList());
        }
    }
}
