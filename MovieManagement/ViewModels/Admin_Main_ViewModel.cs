using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MovieManagement.ViewModels
{
    public class Admin_Main_ViewModel : ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();

        public ObservableCollection<dynamic> Movies { get; set; }
        public Admin_Main_ViewModel() 
        {
            Movies = new ObservableCollection<dynamic>((from m in _context.Movies
                                                        join a in _context.AgeCertificates on m.AgeCertificateId equals a.AgeCertificateId
                                                        join g in _context.Genres on m.GenreId equals g.GenreId
                                                        where m.IsBlockbuster == true
                                                        select new
                                                        {
                                                            m.MovieId,
                                                            m.Title,
                                                            m.ImdbScore,
                                                            a.DisplayContent,
                                                            g.GenreName,
                                                            m.PosterUrl,
                                                        }).ToList());
        }
    }
}
