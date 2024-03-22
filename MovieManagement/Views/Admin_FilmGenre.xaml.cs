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
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MovieManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Admin_FilmGenre : Page
    {
        public Admin_FilmGenre()
        {
            this.InitializeComponent();
            DataContext = new Admin_FilmGenre_ViewModel();
        }
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog Dialog_AddNewMovie = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            Dialog_AddNewMovie.XamlRoot = this.XamlRoot;
            Dialog_AddNewMovie.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_AddNewMovie.Title = "Add New Genre";
            Dialog_AddNewMovie.PrimaryButtonText = "Save";
            Dialog_AddNewMovie.CloseButtonText = "Cancel";
            Dialog_AddNewMovie.DefaultButton = ContentDialogButton.Primary;
            Dialog_AddNewMovie.Content = new MovieGenreDialog();
            Dialog_AddNewMovie.RequestedTheme = (VisualTreeHelper.GetParent(sender as Button) as StackPanel).ActualTheme;

            var result = await Dialog_AddNewMovie.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Debug.Print("User saved their work");
            }
            else
            {
                Debug.Print("User cancelled the dialog");
            }
        }
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ContentDialog Dialog_EditMovie = new ContentDialog();
            Dialog_EditMovie.XamlRoot = this.XamlRoot;
            Dialog_EditMovie.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_EditMovie.Title = "Edit Genre Information";
            Dialog_EditMovie.PrimaryButtonText = "Save";
            Dialog_EditMovie.CloseButtonText = "Cancel";
            Dialog_EditMovie.DefaultButton = ContentDialogButton.Primary;
            Dialog_EditMovie.Content = new MovieGenreDialog();
            Dialog_EditMovie.DataContext = button.DataContext;

            var result = await Dialog_EditMovie.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {

            }
        }
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ContentDialog Dialog_DeleteMovie = new ContentDialog();
            Dialog_DeleteMovie.XamlRoot = this.XamlRoot;
            Dialog_DeleteMovie.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_DeleteMovie.Title = "Confirm Delete Genre";
            Dialog_DeleteMovie.PrimaryButtonText = "Yes";
            Dialog_DeleteMovie.CloseButtonText = "Cancel";
            Dialog_DeleteMovie.DefaultButton = ContentDialogButton.Primary;
            Dialog_DeleteMovie.Content = new ConfirmDeleteDialog();
            var result = await Dialog_DeleteMovie.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {

            }
        }
    }
}
