using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MovieManagement.Models;
using MovieManagement.ViewModels;
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
    public sealed partial class User_DetailTicket : Page
    {
        public int BillID;

        public User_DetailTicket()
        {
            this.InitializeComponent();
        }

        // ========== Get attached data when Navigate from User_Ticket.xaml
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            BillID = GlobalContext.DetailTicketID;
            //Debug.WriteLine("Detial Bill ID : " + BillID);
        }

        // ========== Binding Context to UI
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new User_DetailTicket_ViewModel(BillID);
        }

        // -- Event Click for Back Button
        private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            GlobalContext.SetGo2Ticket(true);
            Frame.Navigate(typeof(Views.User_Ticket));
        }
    }
}
