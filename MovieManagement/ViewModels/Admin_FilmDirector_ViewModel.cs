using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MovieManagement.ViewModels
{
    public class Admin_FilmDirector_ViewModel : ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public ObservableCollection<dynamic> MovieDirectors { get; set; }

        public Admin_FilmDirector_ViewModel() 
        {
            MovieDirectors = new ObservableCollection<dynamic>((from c in _context.Contributors
                                                                join p in _context.People on c.PersonId equals p.PersonId
                                                                join r in _context.Roles on c.RoleId equals r.RoleId
                                                                where r.RoleName == "Director"
                                                                select new
                                                                {
                                                                    p.Fullname,
                                                                    p.AvatarUrl,
                                                                    p.Biography,
                                                                }).Distinct().ToList());
        }
    }
}
