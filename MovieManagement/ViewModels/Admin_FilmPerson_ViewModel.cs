using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace MovieManagement.ViewModels
{
    public class Admin_FilmPerson_ViewModel : ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public ObservableCollection<dynamic> MovieStars { get; set; }
        public Admin_FilmPerson_ViewModel() 
        {
            MovieStars = new ObservableCollection<dynamic>((from p in _context.People
                                                                select new
                                                                {
                                                                    p.PersonId,
                                                                    p.Fullname,
                                                                    p.AvatarUrl,
                                                                    p.Biography,
                                                                }).Distinct().ToList());
        }
     
    }
}
