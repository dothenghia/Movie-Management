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

namespace MovieManagement
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            this.InitializeComponent();

            // Default page is User_Home
            AdminContent.Navigate(typeof(Views.Admin_Main));
        }


        // Navigate to the page corresponding to selected item
        private void NavigationBar_AdminWindow_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem selectedItem = args.SelectedItem as NavigationViewItem;
            if (selectedItem != null)
            {
                switch (selectedItem.Tag.ToString())
                {
                    case "Main_NavgationTag":
                        AdminContent.Navigate(typeof(Views.Admin_Main));
                        break;
                    case "Film_NavgationTag":
                        AdminContent.Navigate(typeof(Views.Admin_Film));
                        break;
                    case "ShowTime_NavgationTag":
                        AdminContent.Navigate(typeof(Views.Admin_ShowTime));
                        break;
                    case "Tickets_NavgationTag":
                        AdminContent.Navigate(typeof(Views.Admin_Tickets));
                        break;
                    case "Voucher_NavgationTag":
                        AdminContent.Navigate(typeof(Views.Admin_Voucher));
                        break;
                    case "Report_NavgationTag":
                        AdminContent.Navigate(typeof(Views.Admin_Report));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
