using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using Windows.Foundation;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Collections;
using Microsoft.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.Input;
using MovieManagement.Views;


namespace MovieManagement.ViewModels
{
    public class User_Movie_ViewModel : ViewModelBase
    {
        // Get database context
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();

        public dynamic MovieInformation { get; set; }
        public ObservableCollection<dynamic> TimesOfDay { get; set;} = new ObservableCollection<dynamic>();
        public ObservableCollection<dynamic> MovieDirectors { get; set; }
        public ObservableCollection<dynamic> MovieActors { get; set; }
        public ObservableCollection<dynamic> Showtimes { get; set; } = new ObservableCollection<dynamic>();
        public ObservableCollection<dynamic> DateInWeek { get; set; } = new ObservableCollection<dynamic>();
        public RelayCommand<string> DateSelectionCommand { get; }
        public RelayCommand<string> ShowtimeSelectionCommand { get; }
        public RelayCommand<string> BookingCommand { get; }
        public ObservableCollection<dynamic> TicketsOfMovie { get; set; }
        public ObservableCollection<dynamic> Seats { get; set; } = new ObservableCollection<dynamic>();
        public string _availableseats;
        public string AvailableSeats
        {
            get { return "Available seats: " + _availableseats; }
            set
            {
                if (_availableseats != value)
                {
                    _availableseats = value;
                    NotifyPropertyChanged(nameof(AvailableSeats));
                }
            }
        }

        public User_Movie_ViewModel(int MovieID) 
        {
            // Execute query to get movie Information
            DateSelectionCommand = new RelayCommand<string>(DateSelection);
            ShowtimeSelectionCommand = new RelayCommand<string>(TimeSelection);
            
            
            MovieInformation = (from m in _context.Movies
                                join a in _context.AgeCertificates on m.AgeCertificateId equals a.AgeCertificateId
                                join g in _context.Genres on m.GenreId equals g.GenreId
                                where m.MovieId == MovieID
                                select new
                                {
                                    m.MovieId,
                                    m.Title,
                                    m.Duration,
                                    m.PublishYear,
                                    m.ImdbScore,
                                    a.DisplayContent,
                                    a.BackgroundColor,
                                    a.ForegroundColor,
                                    m.PosterUrl,
                                    m.TrailerUrl,
                                    m.Description,
                                    g.GenreName
                                }).FirstOrDefault();

            MovieDirectors = new ObservableCollection<dynamic>((from c in _context.Contributors
                                                                join p in _context.People on c.PersonId equals p.PersonId
                                                                join r in _context.Roles on c.RoleId equals r.RoleId
                                                                where c.MovieId == MovieID && r.RoleName == "Director"
                                                                select new
                                                                {
                                                                    p.Fullname,
                                                                    p.AvatarUrl,
                                                                    p.Biography,
                                                                }).ToList());

            MovieActors = new ObservableCollection<dynamic>((from c in _context.Contributors
                                                             join p in _context.People on c.PersonId equals p.PersonId
                                                             join r in _context.Roles on c.RoleId equals r.RoleId
                                                             join m in _context.Movies on c.MovieId equals m.MovieId
                                                             where c.MovieId == MovieID && r.RoleName == "Actor"
                                                             select new
                                                             {
                                                                 p.Fullname,
                                                                 p.AvatarUrl,
                                                                 p.Biography,
                                                             }).ToList());


            //Execute query showtime
            var allShowtimesOfMovie = (from s in _context.ShowTimes where s.MovieId == MovieID
                                       select s).ToList();
            TicketsOfMovie = new ObservableCollection<dynamic>((from t in _context.Tickets 
                                    join s in _context.ShowTimes on t.ShowTimeId equals s.ShowTimeId
                                    where s.MovieId == MovieID select t).ToList());

            foreach (var showtimes in allShowtimesOfMovie)
            {
                if (showtimes.ShowDate >= DateTime.Today && showtimes.ShowDate < DateTime.Today.AddDays(8))
                {
                    Showtimes.Add(new
                    {
                        Date = showtimes.ShowDate.Value.Date.ToString("dd/MM/yyyy"),
                        ShowtimeID = showtimes.ShowTimeId,
                        Time = showtimes.ShowDate.Value.ToString("HH:mm"),
                    }) ;
                }
            }

            for (int i = 0; i < 7; i++)
            {
                DateInWeek.Add(new
                {
                    date = DateTime.Today.AddDays(i).ToString("dd/MM/yyyy")
                }) ;
            }
        }

        private void DateSelection(string selectedDate)
        {
            TimesOfDay.Clear();
            if (selectedDate != null)
            {
                selectedDate = selectedDate.Trim('{','}',' ','=','d','a','t','e');

                var filteredShowtimes = Showtimes.Where(s => s.Date == selectedDate);
                foreach (var showtimes in filteredShowtimes)
                {
                    TimesOfDay.Add(new
                    {
                        time = showtimes.Time,
                        ShowTimeID = showtimes.ShowtimeID,
                        date = showtimes.Date,
                    });
                }
            }
        }

        private void TimeSelection(string selectedTime)
        {
            Seats.Clear();
            if (selectedTime != null)
            {
                selectedTime = selectedTime.Trim('{', '}', ' ', '=', 't', 'i', 'm', 'e');
                int indexOfComma = selectedTime.IndexOf(',');
                selectedTime = selectedTime.Substring(0, indexOfComma);
                var filterShowtimes = TimesOfDay.Where(t => t.time == selectedTime);
                var filterShowtime = filterShowtimes.FirstOrDefault();
                GlobalContext.setShowtime(filterShowtime.ShowTimeID);
                Debug.WriteLine(GlobalContext.showtimeID);
                foreach (var ticket in TicketsOfMovie)
                {
                    if (ticket.ShowTimeId == filterShowtime.ShowTimeID)
                    {
                        Seats.Add(new
                        {
                            position = ticket.Row + ticket.Col,
                            ticketID = ticket.TicketId,
                            isAvailable = ticket.IsAvailable,
                            price = ticket.Price,
                            showtimeID = ticket.ShowTimeId
                        });
                    }
                }
                int count = 0;
                foreach (var seat in Seats)
                {
                    if (seat.isAvailable)
                    {
                        count++;
                    }
                }
                AvailableSeats = count.ToString();
            }
        }

        
    }
}
