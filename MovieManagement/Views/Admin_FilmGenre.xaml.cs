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
using MovieManagement.Models;
using Windows.ApplicationModel.Store.Preview.InstallControl;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MovieManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Admin_FilmGenre : Page
    {       
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        private Admin_FilmGenre_ViewModel viewModel;
        public Admin_FilmGenre()
        {
            this.InitializeComponent();
            viewModel = new Admin_FilmGenre_ViewModel();
            DataContext = viewModel;
            viewModel.Update_Genres();
        }
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog Dialog_AddNewGenre = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            Dialog_AddNewGenre.XamlRoot = this.XamlRoot;
            Dialog_AddNewGenre.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_AddNewGenre.Title = "Add New Genre";
            Dialog_AddNewGenre.Content = new MovieGenreDialog();
            Dialog_AddNewGenre.RequestedTheme = (VisualTreeHelper.GetParent(sender as Button) as StackPanel).ActualTheme;
            await Dialog_AddNewGenre.ShowAsync();
            viewModel.Update_Genres();
        }
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ContentDialog Dialog_EditMovie = new ContentDialog();
            Dialog_EditMovie.XamlRoot = this.XamlRoot;
            Dialog_EditMovie.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_EditMovie.Title = "Edit Genre Information";
            Dialog_EditMovie.Content = new MovieGenreDialog();
            Dialog_EditMovie.DataContext = button.DataContext;
            await Dialog_EditMovie.ShowAsync();
            viewModel.Update_Genres();
        }
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int genreId = (int)button.DataContext.GetType().GetProperty("GenreId").GetValue(button.DataContext);
            ContentDialog Dialog_DeleteGenre = new ContentDialog();
            Dialog_DeleteGenre.XamlRoot = this.XamlRoot;
            Dialog_DeleteGenre.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_DeleteGenre.Title = "Confirm Delete Genre";
            Dialog_DeleteGenre.PrimaryButtonText = "Yes";
            Dialog_DeleteGenre.CloseButtonText = "Cancel";
            Dialog_DeleteGenre.DefaultButton = ContentDialogButton.Primary;
            Dialog_DeleteGenre.Content = new ConfirmDeleteDialog();
            var result = await Dialog_DeleteGenre.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var deleteGenre = _context.Genres.FirstOrDefault(g => g.GenreId == genreId);
                if (deleteGenre != null)
                {
                    _context.Genres.Remove(deleteGenre);
                    _context.SaveChanges();
                }
                viewModel.Update_Genres();
            }
        }
    }
}
