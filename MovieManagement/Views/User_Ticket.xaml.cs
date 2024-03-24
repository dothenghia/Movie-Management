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

            if (GlobalContext.UserID == 0)
            {
                Remind_StackPanel.Visibility = Visibility.Visible;
                Slide_ScrollViewer.Visibility = Visibility.Collapsed;
            }
            else
            {
                Remind_StackPanel.Visibility = Visibility.Collapsed;
                Slide_ScrollViewer.Visibility = Visibility.Visible;
            }
        }

        // ========== Binding Context to UI
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new User_Ticket_ViewModel();
        }


        // ====== Event Click for Login button => Show LoginDialog
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Log in";
            dialog.Content = new LoginDialog();
            (dialog.Content as LoginDialog).LoginValid += OnLoginDialogSuccess;
            await dialog.ShowAsync();
        }
        // ====== Listener for Login Dialog Success event
        private void OnLoginDialogSuccess()
        {
            Frame.Navigate(typeof(User_Setting));
        }



        // ====== Event Click for Signup button => Show SignupDialog
        private async void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Sign up";
            dialog.Content = new SignupDialog();
            (dialog.Content as SignupDialog).SignupValid += OnSignupDialogSuccess;
            await dialog.ShowAsync();
        }
        // ====== Listener for Login Dialog Closed event
        private void OnSignupDialogSuccess()
        {
            Frame.Navigate(typeof(User_Setting));
        }
    }
}
