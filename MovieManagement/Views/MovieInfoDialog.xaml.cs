using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MovieManagement.ViewModels;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using MovieManagement.Models;
using System.Collections.ObjectModel;
using Windows.Storage;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MovieManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MovieInfoDialog : Page
    {
        public List<string> GenresList { get; set; } = new List<string>();
        public List<string> AgeCertificatesList { get; set; } = new List<string>();
        bool isAdd = false;
        bool isEdit = false;
        private StorageFile selectedFile;
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public MovieInfoDialog()
        {
            this.InitializeComponent();
            Admin_FilmInfo_ViewModel filmmodel = new Admin_FilmInfo_ViewModel();
            GenresList = filmmodel.GenresList;
            AgeCertificatesList = filmmodel.AgeCertificatesList;
        }
        private async void UploadPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var window = new Microsoft.UI.Xaml.Window();
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hwnd);

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var bitmapImage = new BitmapImage();
                using (var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    await bitmapImage.SetSourceAsync(fileStream);
                }
                PosterImage.Source = bitmapImage;
            }
            selectedFile = file;

        }
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
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int movieid;
            int.TryParse(InvisibleTextBlock.Text, out movieid);
            string title = Title_TextBox.Text;
            string genrename;
            string agename;
            string description = Description_TextBox.Text;
            string _duration = Duration_TextBox.Text; int duration;
            string _year = Year_TextBox.Text; int year;
            string _score = Score_TextBox.Text; double score;
            string posterUrl = "";
            if (isEdit)
            {
                var profilePicture = PosterImage.Source;
                posterUrl = ExtractUrlFromImageSource(profilePicture);
            }

            //// -- Validate full name
            if (title == null)
            {
                MessageBox.Text = "Please enter a title.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Title_TextBox) as Flyout;
                flyout.ShowAt(Title_TextBox);
                return;
            }

            if (Genre_ComboBox.SelectedValue == null)
            {
                MessageBox.Text = "Please select a genre.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Genre_ComboBox) as Flyout;
                flyout.ShowAt(Genre_ComboBox);
                return;
            }
            genrename = Genre_ComboBox.SelectedValue.ToString();
            var genre = _context.Genres.Where(g => g.GenreName == genrename).FirstOrDefault();
            int genreid = genre.GenreId;

            if (Age_ComboBox.SelectedValue == null)
            {
                MessageBox.Text = "Please select an age certificate.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Age_ComboBox) as Flyout;
                flyout.ShowAt(Age_ComboBox);
                return;
            }
            agename = Age_ComboBox.SelectedValue.ToString();
            var age = _context.AgeCertificates.Where(a => a.DisplayContent == agename).FirstOrDefault();
            int ageid = age.AgeCertificateId;

            Regex regexdouble = new Regex(@"^[0-9]*\.?[0-9]*$");
            if ( _duration == null)
            {
                MessageBox.Text = "Please enter duration.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Duration_TextBox) as Flyout;
                flyout.ShowAt(Duration_TextBox);
                return;
            }
            if (!regexdouble.IsMatch(_duration))
            {
                MessageBox.Text = "Duration must be a number (of minutes).";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Duration_TextBox) as Flyout;
                flyout.ShowAt(Duration_TextBox);
                return;
            }
            int.TryParse(_duration, out duration);
            if ( _score == null)
            {
                MessageBox.Text = "Please enter IMDb score.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Score_TextBox) as Flyout;
                flyout.ShowAt(Score_TextBox);
                return;
            }
            if (!regexdouble.IsMatch(_score))
            {
                MessageBox.Text = "Score must be a number.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Score_TextBox) as Flyout;
                flyout.ShowAt(Score_TextBox);
                return;
            }
            double.TryParse(_score, NumberStyles.Float, CultureInfo.InvariantCulture, out score);
            if (_year == null)
            {
                MessageBox.Text = "Please enter published year.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Year_TextBox) as Flyout;
                flyout.ShowAt(Year_TextBox);
                return;
            }
            if (!regexdouble.IsMatch(_year))
            {
                MessageBox.Text = "Published year must be a number.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Year_TextBox) as Flyout;
                flyout.ShowAt(Year_TextBox);
                return;
            }
            int.TryParse(_year, out year);
            if (selectedFile != null)
            {
                // Copy the selected image file to the assets/avatars folder
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets\\avatars");
                var newFile = await selectedFile.CopyAsync(folder, selectedFile.Name, NameCollisionOption.GenerateUniqueName);
                // Construct the URI for the newly saved image file
                var uri = new Uri($"ms-appx:///Assets/avatars/{newFile.Name}");
                posterUrl = uri.ToString();
                // Reset the selectedFile field to null
                selectedFile = null;
            }
            if (posterUrl == "")
            {
                MessageBox.Text = "Please select a poster image.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Poster_TextBlock) as Flyout;
                flyout.ShowAt(Poster_TextBlock);
                return;
            }
            if (isAdd)
            {
                Movie newMovie = new Movie
                {
                    PosterUrl = posterUrl,
                    Title = title,
                    Description = description,
                    PublishYear = year,
                    ImdbScore = score,
                    Duration = duration,
                    AgeCertificateId = ageid,
                    GenreId = genreid,
                };
                _context.Movies.Add(newMovie);
                _context.SaveChanges();
            }
            else if (isEdit)
            {
                var editmovie = _context.Movies.Where(m => m.MovieId == movieid).FirstOrDefault();
                if (editmovie != null)
                {
                    editmovie.PosterUrl = posterUrl;
                    editmovie.Title = title;
                    editmovie.Description = description;
                    editmovie.PublishYear = year;
                    editmovie.ImdbScore = score;
                    editmovie.Duration = duration;
                    editmovie.AgeCertificateId = ageid;
                    editmovie.GenreId = genreid;
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

        private string ExtractUrlFromImageSource(ImageSource imageSource)
        {
            if (imageSource is BitmapImage bitmapImage)
            {
                Uri uri = bitmapImage.UriSource;
                if (uri != null)
                {
                    return uri.AbsoluteUri;
                }
            }
            return string.Empty;
        }
    }
}
