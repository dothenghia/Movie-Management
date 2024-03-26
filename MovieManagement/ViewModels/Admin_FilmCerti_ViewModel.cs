using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.Graphics.Printing;

namespace MovieManagement.ViewModels
{
    public class Admin_FilmCerti_ViewModel : ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public ObservableCollection<dynamic> Certificates { get; set; }

        public Admin_FilmCerti_ViewModel()
        {
            Certificates = new ObservableCollection<dynamic>();
        }

        public void UpdateCertificate() {
            Certificates.Clear();
            var allCertificates = (from c in _context.AgeCertificates
                                   select new
                                   {
                                       c.AgeCertificateId,
                                       c.DisplayContent,
                                       c.RequireAge,
                                       c.BackgroundColor,
                                       c.ForegroundColor
                                   }).ToList();
            foreach (var certificate in allCertificates)
            {
                Certificates.Add(certificate);
            }
        }
    }
}
