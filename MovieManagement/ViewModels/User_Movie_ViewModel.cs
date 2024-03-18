using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MovieManagement.ViewModels
{
    public class User_Movie_ViewModel:ViewModelBase
    {
        private ObservableCollection<ShowTime> _showtimes;
        private ObservableCollection<Ticket> _tickets;
        public ObservableCollection<ShowTime> ShowTimes
        {
            get
            {
                return _showtimes;
            }
            set
            {
                _showtimes = value;
                NotifyPropertyChanged(nameof(ShowTimes));
            }
        }
        public ObservableCollection<Ticket> Tickets
        {
            get
            {
                return _tickets;
            }
            set
            {
                _tickets = value;
                NotifyPropertyChanged(nameof(Tickets));
            }
        }

        public User_Movie_ViewModel()
        {
            _tickets = new ObservableCollection<Ticket>()
            {
                 new Ticket()
                {
                    IsAvailable = true,
                    Row= "A",
                    Col= 1,
                    Price= 1,
                },
                 new Ticket()
                {
                    IsAvailable = true,
                    Row= "A",
                    Col= 1,
                    Price= 1,
                },
                 new Ticket()
                {
                    IsAvailable = true,
                    Row= "A",
                    Col= 1,
                    Price= 1,
                },
                 new Ticket()
                {
                    IsAvailable = true,
                    Row= "A",
                    Col= 1,
                    Price= 1,
                }
            };
            _showtimes = new ObservableCollection<ShowTime>
            {
                new ShowTime()
                {

                }
            };
        }
    }
}
