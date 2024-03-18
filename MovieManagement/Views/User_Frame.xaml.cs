using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace MovieManagement.Views
{
    public sealed partial class User_Frame : Page
    {
        public User_Frame()
        {
            this.InitializeComponent();
        }


        // Navigate to the page corresponding to selected item
        private void NavigationBar_User_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
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
                        MainContent.Navigate(typeof(Views.User_Profile));
                        break;


                    // Temp Navigation for Test UI
                    case "Movie_NavgationTag":
                        MainContent.Navigate(typeof(Views.User_Movie));
                        break;
                    case "Setting_NavgationTag":
                        MainContent.Navigate(typeof(Views.User_Setting));
                        break;
                    case "Admin_NavgationTag":
                        var currentWindow = (Application.Current as App)?.m_window as MainWindow;
                        var adminWindow = new MainWindow(1);
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
    }
}
