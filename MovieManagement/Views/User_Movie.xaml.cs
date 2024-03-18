using ABI.Windows.ApplicationModel.Activation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
namespace MovieManagement.Views
{
    // Temp Class Data
    public class Showtime
    {
        public int ShowTimeId { get; set; }
        public int MovieId { get; set; }
        public DateTime ShowDate {  get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public Showtime(int showtimeid, int movieid, DateTime showdate) {
            this.ShowTimeId = showtimeid;
            this.MovieId = movieid;
            this.ShowDate = showdate;
        }
    }
    
    // Temp Class Data
    public class Ticket
    {
        public int id {  get; set; }
        public string position { get; set; }
        public bool isAvailable { get; set; }

        public Ticket(int id, string row, string column, bool isAvailable = true)
        {
            this.id = id;
            this.position = row + column;
            this.isAvailable = isAvailable;
        }
    }
    public sealed partial class User_Movie : Page
    {
        
        public ObservableCollection<Showtime> Showtimes { get; } = new ObservableCollection<Showtime>();
        public ObservableCollection<Ticket> Tickets { get; } = new ObservableCollection<Ticket>();
        public ObservableCollection<String> daysOfWeek { get; } = new ObservableCollection<String>();
        public ObservableCollection<String> timesOfDay { get; } = new ObservableCollection<String>();

        public User_Movie()
        {
            this.InitializeComponent();
            this.SizeChanged += OnWindowSizeChanged;
            DateTime currentDate = DateTime.Today;
            for (int i = 0; i < 7; i++)
            {
                daysOfWeek.Add(currentDate.AddDays(i).ToString("dd/MM/yyyy"));
            }

            for (int i = 0; i < 7; i++)
            {
                Showtimes.Add(new Showtime(i, i, new DateTime(2024, 3, 18+i, 19, 00, 15)));
                Showtimes.Add(new Showtime(i, i, new DateTime(2024, 3, 18 + i, 09, 15, 15)));
                Showtimes.Add(new Showtime(i, i, new DateTime(2024, 3, 18 + i, 17, 25, 15)));
                Showtimes.Add(new Showtime(i, i, new DateTime(2024, 3, 18 + i, 19, 40, 15)));
                Showtimes.Add(new Showtime(i, i, new DateTime(2024, 3, 18 + i, 13, 55, 15)));
                Showtimes.Add(new Showtime(i, i, new DateTime(2024, 3, 18 + i, 16, 35, 15)));
            }
            DateGridView.ItemsSource = daysOfWeek;
            for (char j = 'A'; j <= 'C';  j++) 
                for (int i = 0;i < 10;i++)
                {
                    Tickets.Add(new Ticket(i, j.ToString(), i.ToString(),false));
                }
            for (char j = 'D'; j <= 'G'; j++)
                for (int i = 10; i < 20; i++)
                {
                    Tickets.Add(new Ticket(i, j.ToString(), i.ToString(), true));
                }
        }

        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs eventArgs)
        {
            double newWidth = eventArgs.NewSize.Width;
            BezierSegment curve = ScreenLine;
            curve.Point1 = new Point(newWidth / 3, 10);
            curve.Point2 = new Point(newWidth / 3 * 2, 10);
            curve.Point3 = new Point(newWidth, 50);
        }

        // Event Click for Back Button
        private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }


        private void DateGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is string selectedDate)
            {
                timesOfDay.Clear();
                Debug.WriteLine(selectedDate);
                var filteredShowtimes = Showtimes.Where(s => s.ShowDate.Date == DateTime.Parse(selectedDate));
                foreach (var showtime in filteredShowtimes)
                {
                    Debug.WriteLine($"Show time: {showtime.ShowDate.ToString("HH:mm")}");
                    timesOfDay.Add(new String(showtime.ShowDate.ToString("HH:mm")));
                }
                TimeGridView.ItemsSource = timesOfDay;
            }
            SeatsSelectionMap.Visibility = Visibility.Collapsed;
        }
        private void ShowTimes_ItemClick(object sender, ItemClickEventArgs e)
        {
            SeatsSelectionMap.Visibility = Visibility.Visible;
        }
        string seats = "";
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            var selectedToggleButton = sender as ToggleButton;
            TextBlock textBlock = SeatsSelection;
            
            if (selectedToggleButton != null)
            {
                seats += selectedToggleButton.Content.ToString() + " ";
                SeatsSelection.Text = "Your selection: " + seats;
            }
        }

        // Get AttachData when Navigate from User_Home.xaml
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);

        //    if (e.Parameter != null)
        //    {
        //        string movieName = e.Parameter.ToString();
        //        MovieTitle_TextBlock.Text = movieName;
        //    }
        //}
    }
}
