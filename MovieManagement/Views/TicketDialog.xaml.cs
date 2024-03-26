using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MovieManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TicketDialog : Page
    {
        bool isAdd = false;
        bool isEdit = false;
        int selectedMovieId = 0;
        int selectedShowtimeId = 0;
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        List<string> movielist = new List<string>();
        List<string> showtimelist = new List<string>();
        public TicketDialog()
        {
            this.InitializeComponent();
            var allmovie = (from m in _context.Movies
                            select new
                            {
                                m.Title
                            }).ToList();
            foreach (var movie in allmovie)
            {
                movielist.Add(movie.Title);
            }
        }
        private void DialogPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(InvisibleTextBlock.Text)) { isAdd = true; isEdit = false; }
            else { isAdd = false; isEdit = true; }
            if (isEdit)
            {
                Row_TextBox.IsEnabled = false;
                Col_TextBox.IsEnabled = false;
                Movie_SuggestBox.IsEnabled = false;
                Showtime_SuggestBox.IsEnabled = false;
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog parentDialog = this.Parent as ContentDialog;
            if (parentDialog != null)
            {
                parentDialog.Hide();
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int ticketid;
            int.TryParse(InvisibleTextBlock.Text, out ticketid);
            string row = Row_TextBox.Text;
            string _col = Col_TextBox.Text;
            string _price = Price_TextBox.Text;
            int col;
            double price;
            Regex regexrow = new Regex(@"^[A-Z]$");
            Regex regexcol = new Regex(@"^[0-9]*$");
            Regex regexprice = new Regex(@"^[0-9]*\.?[0-9]*$");
            //validate row
            if (row == null)
            {
                MessageBox.Text = "Please enter row.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Row_TextBox) as Flyout;
                flyout.ShowAt(Row_TextBox);
                return;
            }
            if (!regexrow.IsMatch(row))
            {
                MessageBox.Text = "Row must be an alphabet letter.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Row_TextBox) as Flyout;
                flyout.ShowAt(Row_TextBox);
                return;
            }

            //validate col
            if (_col == null)
            {
                MessageBox.Text = "Please enter column.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Col_TextBox) as Flyout;
                flyout.ShowAt(Col_TextBox);
                return;
            }
            if (!regexcol.IsMatch(_col))
            {
                MessageBox.Text = "Row must be a number.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Col_TextBox) as Flyout;
                flyout.ShowAt(Col_TextBox);
                return;
            }
            int.TryParse(_col, out col);

            //validate movie
            if (isAdd && selectedMovieId == 0)
            {
                MessageBox.Text = "Please select a movie.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Movie_SuggestBox) as Flyout;
                flyout.ShowAt(Movie_SuggestBox);
                return;
            }
            //validate showtime
            if (isAdd && selectedShowtimeId == 0 )
            {
                MessageBox.Text = "Please select a showtime.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Showtime_SuggestBox) as Flyout;
                flyout.ShowAt(Showtime_SuggestBox);
                return;
            }
            //validate tickets
            var ticket = _context.Tickets.Where(t => t.ShowTimeId == selectedShowtimeId && t.Row == row && t.Col == col).FirstOrDefault();
            if (isAdd && ticket != null)
            {
                MessageBox.Text = "This ticket already exist.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Showtime_SuggestBox) as Flyout;
                flyout.ShowAt(Showtime_SuggestBox);
                return;
            }

            //validate price
            if (_price == null)
            {
                MessageBox.Text = "Please enter price.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Price_TextBox) as Flyout;
                flyout.ShowAt(Price_TextBox);
                return;
            }
            if (!regexprice.IsMatch(_price))
            {
                MessageBox.Text = "Price must be a number.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Price_TextBox) as Flyout;
                flyout.ShowAt(Price_TextBox);
                return;
            }
            double.TryParse(_price, out price);

            if (isAdd)
            {
                Models.Ticket newTicket = new Models.Ticket
                {
                   Row = row,
                   Col = col,
                   ShowTimeId = selectedShowtimeId,
                   Price = price,
                   IsAvailable = true
                };
                _context.Tickets.Add(newTicket);
                _context.SaveChanges();
            }
            else if (isEdit)
            {
                var editticket = _context.Tickets.Where(t => t.TicketId == ticketid).FirstOrDefault();
                if (editticket != null)
                {
                    editticket.Price = price;
                    _context.SaveChanges();
                }
            }
            // -- Close Dialog and Raise SignupValid event
            ContentDialog parentDialog = this.Parent as ContentDialog;
            if (parentDialog != null)
            {
                parentDialog.Hide();
            }
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var title in movielist)
                {
                    var found = splitText.All((key) =>
                    {
                        return title.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(title);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
            }
        }
        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            string selectedtitle = args.SelectedItem.ToString();
            var movie = _context.Movies.Where(m => m.Title == selectedtitle).FirstOrDefault();
            selectedMovieId = movie.MovieId;
            Showtime_SuggestBox.IsEnabled = true;
            showtimelist.Clear();
            var showtimes = (from s in _context.ShowTimes
                            where s.MovieId == selectedMovieId && s.ShowDate >= DateTime.Now
                            select new
                            {
                                ShowDate = s.ShowDate.Value.ToString()
                            }).ToList();
            foreach (var showtime in showtimes)
            {
                showtimelist.Add(showtime.ShowDate);
            }
        }
        private void ShowtimeSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var showtime in showtimelist)
                {
                    var found = splitText.All((key) =>
                    {
                        return showtime.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(showtime);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
            }
        }
        private void ShowtimeSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            string selectedshowtime = args.SelectedItem.ToString();
            var showtime = _context.ShowTimes.Where(s => s.ShowDate.Value.ToString() == selectedshowtime && s.MovieId == selectedMovieId).FirstOrDefault();
            selectedShowtimeId = showtime.ShowTimeId;
        }
    }
}
