using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManagement.ViewModels
{
    public class Admin_FilmContributor_ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();

        public ObservableCollection<dynamic> Contributors { get; set; }
        public Admin_FilmContributor_ViewModel()
        {
            Contributors = new ObservableCollection<dynamic>();
        }

        public void Update_Contributors()
        {
            Contributors.Clear();
            var contributors = (from c in _context.Contributors
                                join m in _context.Movies on c.MovieId equals m.MovieId
                                join p in _context.People on c.PersonId equals p.PersonId
                                join r in _context.Roles on c.RoleId equals r.RoleId
                            select new
                            {
                                c.MovieId,
                                c.PersonId,
                                c.RoleId,
                                m.PosterUrl,
                                m.Title,
                                p.AvatarUrl,
                                p.Fullname,
                                r.RoleName
                            }).ToList();
            foreach (var contribute in contributors)
            {
                Contributors.Add(contribute);
            }

        }
    }
}
