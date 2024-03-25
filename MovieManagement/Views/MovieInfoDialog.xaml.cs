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
        public MovieInfoDialog()
        {
            this.InitializeComponent();
            Admin_FilmInfo_ViewModel instance = new Admin_FilmInfo_ViewModel();
            GenresList = instance.GenresList;
            AgeCertificatesList = instance.AgeCertificatesList;

        }
        private async void UploadPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();


            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var window = new Microsoft.UI.Xaml.Window();
            // ...
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
            else
            {
                //PickAPhotoOutputTextBlock.Text = "Operation cancelled.";
            }

        }
    }
}
