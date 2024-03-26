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
    public sealed partial class MovieContributorDialog : Page
    {
        bool isAdd = false;
        bool isEdit = false;
        int selectedMovieId = 0;
        int selectedPersonId = 0;
        int roleid = 0;
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        List<string> movielist = new List<string>();
        List<string> personlist = new List<string>();
        public MovieContributorDialog()
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
            var allperson = (from p in _context.People
                            select new
                            {
                                p.Fullname
                            }).ToList();
            foreach (var person in allperson)
            {
                personlist.Add(person.Fullname);
            }
        }
        private void DialogPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(InvisibleMovieId.Text)) { isAdd = true; isEdit = false; }
            else { isAdd = false; isEdit = true; }
            if (isEdit)
            {
                Movie_SuggestBox.IsEnabled = false;
                Person_SuggestBox.IsEnabled = false;
                int.TryParse(InvisibleMovieId.Text, out selectedMovieId);
                int.TryParse(InvisiblePersonId.Text, out selectedPersonId);
                int.TryParse(InvisibleRoleId.Text, out roleid);
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
            //validate movie
            if (selectedMovieId == 0)
            {
                MessageBox.Text = "Please select a movie.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Movie_SuggestBox) as Flyout;
                flyout.ShowAt(Movie_SuggestBox);
                return;
            }
            ////validate person
            if (selectedPersonId == 0)
            {
                MessageBox.Text = "Please select a Person.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Person_SuggestBox) as Flyout;
                flyout.ShowAt(Person_SuggestBox);
                return;
            }
            if (Role_ComboBox.SelectedValue == null)
            {
                MessageBox.Text = "Please select a role.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Role_ComboBox) as Flyout;
                flyout.ShowAt(Role_ComboBox);
                return;
            }
            if (Role_ComboBox.SelectedValue.ToString() == "Director") roleid = 1;
            if (Role_ComboBox.SelectedValue.ToString() == "Actor") roleid = 2;

            ///validate contributor
            var contributor = _context.Contributors.Where(c => c.MovieId == selectedMovieId && c.PersonId == selectedPersonId && c.RoleId == roleid).FirstOrDefault();
            if (contributor != null)
            {
                MessageBox.Text = "This person already contribute to this film with this role.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Role_ComboBox) as Flyout;
                flyout.ShowAt(Role_ComboBox);
                return;
            }

            if (isAdd)
            {
                Contributor newContributor = new Contributor
                {
                    MovieId = selectedMovieId,
                    PersonId = selectedPersonId,
                    RoleId = roleid,
                };
                _context.Contributors.Add(newContributor);
                _context.SaveChanges();
            }
            // -- Close Dialog and Raise SignupValid event
            ContentDialog parentDialog = this.Parent as ContentDialog;
            if (parentDialog != null)
            {
                parentDialog.Hide();
            }
        }

        private void MovieSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
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
        private void MovieSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            string selectedtitle = args.SelectedItem.ToString();
            var movie = _context.Movies.Where(m => m.Title == selectedtitle).FirstOrDefault();
            selectedMovieId = movie.MovieId;
        }
        private void PersonSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var showtime in personlist)
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
        private void PersonSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            string selectedperson = args.SelectedItem.ToString();
            var person = _context.People.Where(s => s.Fullname == selectedperson).FirstOrDefault();
            selectedPersonId = person.PersonId;
        }
    }
}
