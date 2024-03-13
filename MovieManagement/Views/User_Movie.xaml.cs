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
        public string Index { get; set; }
        public string Date {  get; set; }
        public string Time { get; set; }
        public string Image {  get; set; }

        public Showtime(string index, string date, string time) {
            this.Index = index;
            this.Date = date;
            this.Time = time;
        }
    }
    
    // Temp Class Data
    public class Ticket
    {
        public int id {  get; set; }
        public string position { get; set; }

        public Ticket(int id, string row, string column) {
            this.id = id;
            this.position = row + column;
        }
    }
    
    public sealed partial class User_Movie : Page
    {
        public ObservableCollection<Showtime> Showtimes { get; } = new ObservableCollection<Showtime>();
        public ObservableCollection<Ticket> Tickets { get; } = new ObservableCollection<Ticket>();
        public User_Movie()
        {
            this.InitializeComponent();
            for (int i = 0; i < 10; i++)
            {
                Showtimes.Add(new Showtime(i.ToString(), "19/3/2024", "11:30"));
            }
            DateGridView.ItemsSource = Showtimes;
            TimeGridView.ItemsSource = Showtimes;

            for (char j = 'A'; j <= 'C';  j++) 
                for (int i = 0;i < 10;i++)
                {
                    Tickets.Add(new Ticket(i, j.ToString(), i.ToString()));
                }
            SeatGridView.ItemsSource = Tickets;
        }

        // Event Click for Back Button
        private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.GoBack();
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
