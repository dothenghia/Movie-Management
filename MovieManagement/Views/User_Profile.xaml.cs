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
            (dialog.Content as LoginDialog).LoginDialogClosed += OnLoginDialogClosed;
            await dialog.ShowAsync();
        }

        // ====== Listener for Login Dialog Closed event
        private void OnLoginDialogClosed()
        {
            Debug.WriteLine("Login Dialog Closed");
            GlobalContext.SetGo2Setting(true);
            GlobalContext.SetUserID(1);
            Frame.Navigate(typeof(User_Setting));
        }

















        // ====== Event Click for Signup button
        private async void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Sign up";
            dialog.PrimaryButtonText = "Sign up";
            dialog.CloseButtonText = "Cancel";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = new SignupDialog();

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var currentWindow = (Application.Current as App)?.m_window as MainWindow;
                var adminWindow = new MainWindow(1);
                adminWindow.Activate();
                currentWindow.Close();
            }
        }
    }
}
