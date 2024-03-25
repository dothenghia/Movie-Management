using Microsoft.UI.Windowing;
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.Foundation.Collections;



namespace MovieManagement.Views
{
    public sealed partial class User_Frame : Page
    {
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public ObservableCollection<dynamic> Movies { get; set; }
        public ObservableCollection<dynamic> Person { get; set; }
        private List<string> movie_name { get; set; } = new List<string>();
        private List<string> person_name { get; set; } = new List<string>();
        public User_Frame()
        {
            this.InitializeComponent();
            Movies = new ObservableCollection<dynamic>((from m in _context.Movies
                      select new
                      {
                          m.Title,
                      }).ToList());
            foreach(var movie in Movies)
            {
                movie_name.Add(movie.Title);
            }
            Person = new ObservableCollection<dynamic>((from p in _context.People
                                                        select new
                                                        {
                                                            p.Fullname,
                                                        }).ToList());
            foreach (var person in Person)
            {
                person_name.Add(person.Fullname);
            }
     
        }


        // Navigate to the page corresponding to selected item
        private void NavigationBar_User_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            // Fix navigation to Setting page after login bug
            if (GlobalContext.Go2Setting) {
                MainContent.Navigate(typeof(Views.User_Setting));
                GlobalContext.SetGo2Setting(false);

                // Set NavigationViewItem to Profile
                NavigationViewItem profileItem = NavigationBar_User.FooterMenuItems[0] as NavigationViewItem;
                if (profileItem != null) {
                    NavigationBar_User.SelectedItem = profileItem;
                }

                return;
            }


            NavigationViewItem selectedItem = args.SelectedItem as NavigationViewItem;
            if (selectedItem != null)
            {
                switch (selectedItem.Tag.ToString())
                {
                    case "Home_NavgationTag":
                        MainContent.Navigate(typeof(Views.User_Home));
                        break;
                    case "Ticket_NavgationTag":
                        MainContent.Navigate(typeof(Views.User_Ticket));
                        break;
                    case "Profile_NavgationTag":
                        if (GlobalContext.UserID == 0) { // If user is not logged in
                            MainContent.Navigate(typeof(Views.User_Profile));
                        }
                        else { // If user is logged in
                            MainContent.Navigate(typeof(Views.User_Setting));
                        }
                        break;


                    // Temp Navigation for Test UI
                    case "Admin_NavgationTag":
                        var currentWindow = (Application.Current as App)?.m_window as MainWindow;
                        var adminWindow = new AdminWindow();
                        adminWindow.Activate();
                        currentWindow.Close();
                        break;
                    case "ExportTicket_NavgationTag":
                        MainContent.Navigate(typeof(Views.User_ExportTicket));
                        break;


                    default:
                        MainContent.Navigate(typeof(Views.User_Home));
                        break;
                }
            }
        }

        private void Search_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new List<string>();
                var splitText = sender.Text.ToLower().Split(' ');
                foreach ( var item in person_name)
                {
                    var found = splitText.All((key) =>{
                        return item.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(item);
                    }
                }
                foreach ( var item in movie_name) 
                {
                    var found = splitText.All((key) =>
                    {
                        return item.ToLower().Contains(key);
                    });
                    if ( found)
                    {
                        suitableItems.Add(item);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No resuslt found");
                }
                sender.ItemsSource = suitableItems;
            }
        }

        private void Search_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            SuggestionOutput.Text = args.SelectedItem.ToString();
            GlobalContext.name = SuggestionOutput.Text;
        }

        private void Search_QuerySummited(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            MainContent.Navigate(typeof(User_Search));
        }
    }
}
