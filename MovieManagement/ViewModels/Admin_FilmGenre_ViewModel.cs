using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MovieManagement.ViewModels
{
    public class Admin_FilmGenre_ViewModel : ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public ObservableCollection<dynamic> Genres { get; set; }
        public Admin_FilmGenre_ViewModel() 
        {
            Genres = new ObservableCollection<dynamic>((from g in _context.Genres
                                                         select new
                                                         {
                                                             g.GenreId,
                                                             g.GenreName
                                                         }).ToList());
        }
    }
}
