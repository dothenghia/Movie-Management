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
    public sealed partial class User_Profile : Page
    {
        public User_Profile()
        {
            this.InitializeComponent();
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
            Frame.Navigate(typeof(User_Home));
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
            Frame.Navigate(typeof(User_Home));
        }
    }
}
