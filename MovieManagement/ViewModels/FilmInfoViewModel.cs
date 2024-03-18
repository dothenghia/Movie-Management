using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManagement.ViewModels
{
    //backing data source in MyViewModel
    public class FilmInfoViewModel
    {
        public String Title { get; set; }
        public String Genre { get; set; }
        public Double Duration { get; set; }
        public int PublishedYear { get; set; }
        public Double IMDBRating { get; set; }
        public String Certification { get; set; }
        public String ImagePath { get; set; }

        public FilmInfoViewModel(String Title, String Genre, Double Duration, int PublishedYear, Double IMDBRating, 
            String Certification, String ImagePath)
        {
            this.Title = Title;
            this.Genre = Genre;
            this.Duration = Duration;
            this.PublishedYear = PublishedYear;
            this.IMDBRating = IMDBRating;
            this.Certification = Certification;
            this.ImagePath = ImagePath;
        }

        public static List<FilmInfoViewModel> FimlInfos()
        {
            return new List<FilmInfoViewModel>(new FilmInfoViewModel[4] {
            new FilmInfoViewModel("Phim moi1","Hihi",100,2013,4.5,"11+","hululu"),
            new FilmInfoViewModel("Phim moi2","Hihi",130,2203,6.5,"109+","hululu"),
            new FilmInfoViewModel("Phim moi3","Hihi",170,2103,8.5,"129+","hululu"),
            new FilmInfoViewModel("Phim moi4","Hihi",190,2013,0.5,"100+","hululu")
        });
        }
    }
}
