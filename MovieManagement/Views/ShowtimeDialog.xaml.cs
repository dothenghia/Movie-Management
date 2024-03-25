using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MovieManagement.Converters;
using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
    public sealed partial class ShowtimeDialog : Page
    {
        bool isAdd = false;
        bool isEdit = false;
        int selectedMovieId = 0;
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        List<string> movielist = new List<string>();
        public ShowtimeDialog()
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
                Movie_SuggestBox.Text = InvisibleTextBlock2.Text;
                int.TryParse(InvisibleTextBlock3.Text, out selectedMovieId);
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
            int showtimeid;
            int.TryParse(InvisibleTextBlock.Text, out showtimeid);
            int maxrow;
            int.TryParse(MaxRow_TextBox.Text, out maxrow);
            int maxcol ;
            int.TryParse(MaxCol_TextBox.Text, out maxcol);

            ///Validate date
            DateTimeOffset? selectedDate = ShowDatePicker.SelectedDate;
            DateTime defaultDate = new DateTime(1601, 1, 1);
            DateTime today = DateTime.Today;
            ///Validate time
            TimeSpan? selectedTime = ShowTimePicker.SelectedTime;

            if (selectedDate.Value.Date == defaultDate)
            {
                MessageBox.Text = "Please select a day.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(ShowDatePicker) as Flyout;
                flyout.ShowAt(ShowDatePicker);
                return;
            }
            if (selectedDate.Value.Date <= today.Date)
            {
                MessageBox.Text = "Can not add showtime for previous day.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(ShowDatePicker) as Flyout;
                flyout.ShowAt(ShowDatePicker);
                return;
            }
            DateTime selectedShowTime = selectedDate.Value.DateTime;
            selectedShowTime = new DateTime(selectedShowTime.Year,
                                            selectedShowTime.Month,
                                            selectedShowTime.Day,
                                            0,0,0);
            selectedShowTime += selectedTime.Value;
            //validate movie
            if (selectedMovieId == 0)
            {
                MessageBox.Text = "Please select a movie.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Movie_SuggestBox) as Flyout;
                flyout.ShowAt(Movie_SuggestBox);
                return;
            }
            //validate showtime
            var showtime = _context.ShowTimes.Where(s => s.MovieId == selectedMovieId && s.ShowDate == selectedShowTime).FirstOrDefault();
            if (showtime != null)
            {
                MessageBox.Text = "This movie already has this showtime.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Movie_SuggestBox) as Flyout;
                flyout.ShowAt(Movie_SuggestBox);
                return;
            }

            if (isAdd)
            {
                ShowTime newShowtime = new ShowTime
                {
                    MovieId = selectedMovieId,
                    MaxCol = maxcol,
                    MaxRow = maxrow,
                    ShowDate = selectedShowTime,
                };
                _context.ShowTimes.Add(newShowtime);
                _context.SaveChanges();
            }
            else if (isEdit)
            {
                var editshowtime = _context.ShowTimes.Where(s => s.ShowTimeId == showtimeid).FirstOrDefault();
                if (editshowtime != null)
                {
                    editshowtime.MovieId = selectedMovieId;
                    editshowtime.MaxCol = maxcol;
                    editshowtime.MaxRow = maxrow;
                    editshowtime.ShowDate = selectedShowTime;
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
            var movie = _context.Movies.Where(m=> m.Title == selectedtitle).FirstOrDefault();
            selectedMovieId = movie.MovieId;
        }
    }
}
