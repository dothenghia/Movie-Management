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
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.UserProfile;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MovieManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MovieGenreDialog : Page
    {
        bool isAdd = false;
        bool isEdit = false;
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public MovieGenreDialog()
        {
            this.InitializeComponent();
        }
        //check mode add/edit
        private void DialogPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(InvisibleTextBlock.Text)) { isAdd = true; isEdit = false; }
            else { isAdd = false; isEdit = true; }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog parentDialog = this.Parent as ContentDialog;
            if (parentDialog != null)
            {
                parentDialog.Hide();
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int genreid;
            int.TryParse(InvisibleTextBlock.Text, out genreid);
            string genrename = GenreName_TextBox.Text;

            if (genrename == null || genrename.Length == 0)
            {
                MessageBox.Text = "Genre name must not be null.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(GenreName_TextBox) as Flyout;
                flyout.ShowAt(GenreName_TextBox);
                return;
            }

            //// -- Validate username, password and fullname, DOB
            Regex regex = new Regex(@"^[A-Za-z ]*$");
            if (!regex.IsMatch(genrename))
            {
                MessageBox.Text = "Genre name must contains letters only.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(GenreName_TextBox) as Flyout;
                flyout.ShowAt(GenreName_TextBox);
                return;
            }

            // -- Check if database already has this genrename
            var genre = _context.Genres.Where(g => g.GenreName == genrename).FirstOrDefault();
            if (genre != null)
            {
                MessageBox.Text = "This genre already exists.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(GenreName_TextBox) as Flyout;
                flyout.ShowAt(GenreName_TextBox);
                return;
            }
            if (isAdd)
            {
                Genre newGenre = new Genre
                {
                    GenreName = genrename,
                };
                _context.Genres.Add(newGenre);
                _context.SaveChanges();
            }
            else if (isEdit)
            {
                var editgenre = _context.Genres.Where(g => g.GenreId == genreid).FirstOrDefault();
                if (editgenre != null)
                {
                    editgenre.GenreName = genrename;
                    _context.SaveChanges();
                }
            }
            
            // -- Close Dialog and Raise SignupValid event
            ContentDialog parentDialog = this.Parent as ContentDialog;
            if (parentDialog != null)
            {
                parentDialog.Hide();
            }
        }
    }
}
