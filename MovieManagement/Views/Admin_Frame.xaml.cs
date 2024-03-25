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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MovieManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Admin_Frame : Page
    {
        public Admin_Frame()
        {
            this.InitializeComponent();

            // Default page is Admin_Main
            MainContent.Navigate(typeof(Views.Admin_Main));
        }


        // Navigate to the page corresponding to selected item
        private void NavigationBar_Admin_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (NavigationViewItem)args.SelectedItem;
            string selectedItemTag = (string)selectedItem.Tag;

            switch (selectedItemTag)
            {
                case "Main_NavTag":
                    MainContent.Navigate(typeof(Views.Admin_Main));
                    break;
                case "FilmInfo_NavTag":
                    MainContent.Navigate(typeof(Views.Admin_FilmInfo));
                    break;
                case "FilmGenre_NavTag":
                    MainContent.Navigate(typeof(Views.Admin_FilmGenre));
                    break;
                case "FilmCerti_NavTag":
                    MainContent.Navigate(typeof(Views.Admin_FilmCerti));
                    break;
                case "FilmPerson_NavTag":
                    MainContent.Navigate(typeof(Views.Admin_FilmPerson));
                    break;
                case "ShowTime_NavTag":
                    MainContent.Navigate(typeof(Views.Admin_ShowTime));
                    break;
                case "Tickets_NavTag":
                    MainContent.Navigate(typeof(Views.Admin_Tickets));
                    break;
                case "Voucher_NavTag":
                    MainContent.Navigate(typeof(Views.Admin_Voucher));
                    break;
                case "Report_NavTag":
                    MainContent.Navigate(typeof(Views.Admin_Report));
                    break;
                default:
                    break;
            }
        }
    }
}
