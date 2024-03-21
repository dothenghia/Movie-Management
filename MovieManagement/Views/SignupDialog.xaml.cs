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
    public delegate void SignupDialogClosedEventHandler();

    public sealed partial class SignupDialog : Page
    {
        // ====== Define public events and variables
        public event SignupDialogClosedEventHandler SignupValid; // SignupValid event to be raised when Signup is valid
        private DB_MovieManagementContext _context = new DB_MovieManagementContext(); // Database context

        public SignupDialog()
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


        // ====== Event Click for Signup button
        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            string username = Username_TextBox.Text;
            string password = Password_PasswordBox.Password;
            string fullname = Fullname_TextBox.Text;
            string gender = (bool)Male_RadioButton.IsChecked ? "Male" : "Female";

            // -- Validate username, password and fullname, DOB
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
            if (fullname.Length < 3 || fullname.Length > 30)
            {
                MessageBox.Text = "Fullname must have length of 3 to 30 characters.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Fullname_TextBox) as Flyout;
                flyout.ShowAt(Fullname_TextBox);
                return;
            }
            if (DOB_DatePicker.Date > DateTime.Now || DOB_DatePicker.SelectedDate == null)
            {
                MessageBox.Text = "Please select a valid date of birth.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(DOB_DatePicker) as Flyout;
                flyout.ShowAt(DOB_DatePicker);
                return;
            }

            // -- Check if database already has this username
            var user = _context.Accounts.Where(a => a.Username == username).FirstOrDefault();
            if (user != null) {
                MessageBox.Text = "Username already exists.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Username_TextBox) as Flyout;
                flyout.ShowAt(Username_TextBox);
                return;
            }

            // -- Hash password
            string hashedPassword = HashPassword(password);

            // -- Add new user to database
            Account newAccount = new Account
            {
                Username = username,
                Password = hashedPassword,
                Fullname = fullname,
                Dob = DOB_DatePicker.Date.DateTime,
                Gender = gender,
                IsAdmin = false
            };
            _context.Accounts.Add(newAccount);
            _context.SaveChanges();

            // -- Close Dialog and Raise SignupValid event
            ContentDialog parentDialog = this.Parent as ContentDialog;
            if (parentDialog != null)
            {
                parentDialog.Hide();
                GlobalContext.SetGo2Setting(true);
                GlobalContext.SetUserID(newAccount.AccountId);
                SignupValid?.Invoke();
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
