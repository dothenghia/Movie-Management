using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MovieManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace MovieManagement.Views
{
    public sealed partial class User_Ticket : Page
    {
        public User_Ticket()
        {
            this.InitializeComponent();
        }

        // ========== Binding Context to UI
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new User_Ticket_ViewModel();
        }
    }
}
