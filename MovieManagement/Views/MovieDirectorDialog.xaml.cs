using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using MovieManagement.ViewModels;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using MovieManagement.Models;
using System.Text.RegularExpressions;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MovieManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MovieDirectorDialog : Page
    {
        bool isAdd = false;
        bool isEdit = false;
        private StorageFile selectedFile;
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public MovieDirectorDialog()
        {
            this.InitializeComponent();
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
                AvatarImage.ProfilePicture = bitmapImage;
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
            int personid;
            int.TryParse(InvisibleTextBlock.Text, out personid);
            string fullname = Fullname_TextBox.Text;
            string bio = Bio_TextBox.Text;
            string avatarUrl ="";
            if (isEdit) { avatarUrl = AvatarImage.ProfilePicture.ToString(); }
            
            //// -- Validate full name
            if (fullname == null || fullname.Length == 0)
            {
                MessageBox.Text = "Full name must not be null.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Fullname_TextBox) as Flyout;
                flyout.ShowAt(Fullname_TextBox);
                return;
            }
            Regex regex = new Regex(@"^[A-Za-z ]*$");
            if (!regex.IsMatch(fullname))
            {
                MessageBox.Text = "Full name must contains letters only.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Fullname_TextBox) as Flyout;
                flyout.ShowAt(Fullname_TextBox);
                return;
            }

            // -- Check if database already has this name
            var ppl = _context.People.Where(p => p.Fullname == fullname).FirstOrDefault();
            if (ppl != null)
            {
                MessageBox.Text = "This person already exists.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Fullname_TextBox) as Flyout;
                flyout.ShowAt(Fullname_TextBox);
                return;
            }
            if (selectedFile != null)
            {
                // Copy the selected image file to the assets/avatars folder
                var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets\\avatars");
                var newFile = await selectedFile.CopyAsync(folder, selectedFile.Name, NameCollisionOption.GenerateUniqueName);
                // Construct the URI for the newly saved image file
                var uri = new Uri($"ms-appx:///Assets/avatars/{newFile.Name}");
                avatarUrl = uri.ToString();
                // Reset the selectedFile field to null
                selectedFile = null;
            }
            if (avatarUrl == "") {
                MessageBox.Text = "Please select an avatar image.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Avartar_StackPanel) as Flyout;
                flyout.ShowAt(Avartar_StackPanel);
                return;
            }
            if (isAdd)
            {
                Person newPerson = new Person
                {
                    Fullname = fullname,
                    Biography = bio,
                    AvatarUrl = avatarUrl
                };
                _context.People.Add(newPerson);
                _context.SaveChanges();
            }
            else if (isEdit)
            {
                var editperson = _context.People.Where(p => p.PersonId == personid).FirstOrDefault();
                if (editperson != null)
                {
                    editperson.Fullname = fullname;
                    editperson.Biography = bio;
                    editperson.AvatarUrl = avatarUrl;
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
