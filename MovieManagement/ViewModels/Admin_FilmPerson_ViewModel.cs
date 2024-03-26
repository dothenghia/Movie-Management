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
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml.Automation.Peers;

namespace MovieManagement.ViewModels
{
    public class Admin_FilmPerson_ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();
        
        public ObservableCollection<dynamic> MovieStars { get; set; }
        public Admin_FilmPerson_ViewModel() 
        {
            MovieStars = new ObservableCollection<dynamic>((from p in _context.People
                                                            select p).ToList());
        }

        public void Update_Person()
        {
            MovieStars.Clear();
            var allStars = (from p in _context.People
                           select new
                           {
                               p.PersonId,
                               p.AvatarUrl,
                               p.Biography,
                               p.Fullname
                           }).ToList();
            foreach (var person in allStars)
            {
                MovieStars.Add(person);
            }

        }
     
    }
}
