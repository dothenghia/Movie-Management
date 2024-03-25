using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
    public delegate void LoginDialogClosedEventHandler();

    public sealed partial class LoginDialog : Page
    {
        // ====== Define public events and variables
        public event LoginDialogClosedEventHandler LoginValid; // LoginValid event to be raised when login is valid
        private DB_MovieManagementContext _context = new DB_MovieManagementContext(); // Database context

        public LoginDialog()
        {
            this.InitializeComponent();
        }

        // ====== Event Click for Cancel button
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog parentDialog = this.Parent as ContentDialog;
            if (parentDialog != null) {
                parentDialog.Hide();
            }
        }


        // ====== Event Click for Login button
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = Username_TextBox.Text;
            string password = Password_PasswordBox.Password;

            // -- Validate username and password
            Regex regex = new Regex(@"^[A-Za-z0-9]{3,20}$");
            if (!regex.IsMatch(username))
            {
                MessageBox.Text = "Username must contain only letters and numbers \nAnd have length of 3 to 20 characters.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Username_TextBox) as Flyout;
                flyout.ShowAt(Username_TextBox);
                return;
            }
            if (!regex.IsMatch(password))
            {
                MessageBox.Text = "Password must contain only letters and numbers \nAnd have length of 3 to 20 characters.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Password_PasswordBox) as Flyout;
                flyout.ShowAt(Password_PasswordBox);
                return;
            }

            // -- Hash password
            string hashedPassword = HashPassword(password);

            // -- Check with database for valid login
            var user = _context.Accounts.Where(a => a.Username == username && a.Password == hashedPassword).FirstOrDefault();
            if (user != null) {
                Debug.WriteLine(user.AccountId);
                Debug.WriteLine(user.IsAdmin);

                if ((bool)user.IsAdmin) {
                    ContentDialog parentDialog = this.Parent as ContentDialog;
                    if (parentDialog != null) {
                        parentDialog.Hide();
                    }
                    var currentWindow = (Application.Current as App)?.m_window as MainWindow;
                    var adminWindow = new AdminWindow();
                    adminWindow.Activate();
                    currentWindow.Close();
                }
                else {
                    ContentDialog parentDialog = this.Parent as ContentDialog;
                    if (parentDialog != null) {
                        parentDialog.Hide();
                        GlobalContext.SetUserID(user.AccountId);
                        LoginValid?.Invoke();
                    }
                }
            }
            else {
                MessageBox.Text = "Invalid username or password";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Login_Button) as Flyout;
                flyout.ShowAt(Login_Button);
                return;
            }


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
