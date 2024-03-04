using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace MovieManagement
{

    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // Default page is User_Home
            MainContent.Navigate(typeof(Views.User_Home));
        }


        // Navigate to the page corresponding to selected item
        private void NavigationBar_MainWindow_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
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
                    default:
                        break;
                }
            }
        }


    }
}
