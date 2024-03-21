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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace MovieManagement.Views
{
    public sealed partial class User_Setting : Page
    {
        public User_Setting()
        {
            this.InitializeComponent();
        }

        // ========== Binding Context to UI
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new User_Setting_ViewModel();
        }


        // ========== Event Click for Profile button
        private async void Profile_Click(object sender, RoutedEventArgs e)
        {
            string fullname = Fullname_TextBox.Text;

            // -- Validate fullname
            if (fullname.Length < 3 || fullname.Length > 30)
            {
                MessageBox.Text = "Fullname must have length of 3 to 30 characters.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Fullname_TextBox) as Flyout;
                flyout.ShowAt(Fullname_TextBox);
                return;
            }

            // -- Update fullname
            (DataContext as User_Setting_ViewModel)?.UpdateFullName(fullname);

            // -- Show success message
            ContentDialog contentDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Success",
                Content = "Your Full name has been updated.",
                CloseButtonText = "Ok"
            };
            await contentDialog.ShowAsync();
        }


        // ========== Event Click for Change Password button
        private async void Password_Click(object sender, RoutedEventArgs e)
        {
            string currentPassword = Currentpass_PasswordBox.Password;
            string newPassword = Newpass_PasswordBox.Password;
            string confirmPassword = Confirmpass_PasswordBox.Password;

            // -- Validate password
            Regex regex = new Regex(@"^[A-Za-z0-9]{3,20}$");
            if (!regex.IsMatch(currentPassword))
            {
                MessageBox.Text = "Password must contain only letters and numbers \nAnd have length of 3 to 20 characters.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Currentpass_PasswordBox) as Flyout;
                flyout.ShowAt(Currentpass_PasswordBox);
                return;
            }
            if (!regex.IsMatch(newPassword))
            {
                MessageBox.Text = "Password must contain only letters and numbers \nAnd have length of 3 to 20 characters.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Newpass_PasswordBox) as Flyout;
                flyout.ShowAt(Newpass_PasswordBox);
                return;
            }
            if (newPassword != confirmPassword)
            {
                MessageBox.Text = "New password and confirm password must be the same.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Confirmpass_PasswordBox) as Flyout;
                flyout.ShowAt(Confirmpass_PasswordBox);
                return;
            }

            // -- Hash password
            string hashedCurrentPassword = HashPassword(currentPassword);

            // -- Check if current password is correct
            if (!(DataContext as User_Setting_ViewModel)?.CheckPassword(hashedCurrentPassword) ?? false)
            {
                MessageBox.Text = "Current password is incorrect.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Currentpass_PasswordBox) as Flyout;
                flyout.ShowAt(Currentpass_PasswordBox);
                return;
            }

            // -- Hash new password
            string hashedNewPassword = HashPassword(newPassword);

            // -- Update password
            (DataContext as User_Setting_ViewModel)?.UpdatePassword(hashedNewPassword);

            // -- Show success message
            ContentDialog contentDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Success",
                Content = "Your password has been updated.",
                CloseButtonText = "Ok"
            };
            await contentDialog.ShowAsync();
        }

        // ====== Hash password using SHA256 algorithm
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
