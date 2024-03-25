using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MovieManagement.ViewModels;
using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MovieManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Admin_FilmInfo : Page
    {
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        private Admin_FilmInfo_ViewModel _viewModel;
        public Admin_FilmInfo()
        {
            this.InitializeComponent();
            _viewModel = new Admin_FilmInfo_ViewModel();
            DataContext = _viewModel;
            _viewModel.Update_FilmInfo();
        }
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog Dialog_AddNewMovie = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            Dialog_AddNewMovie.XamlRoot = this.XamlRoot;
            Dialog_AddNewMovie.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_AddNewMovie.Title = "Add New Movie";
            Dialog_AddNewMovie.Content = new MovieInfoDialog();
            Dialog_AddNewMovie.RequestedTheme = (VisualTreeHelper.GetParent(sender as Button) as StackPanel).ActualTheme;
            await Dialog_AddNewMovie.ShowAsync();
            _viewModel.Update_FilmInfo();
        }
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ContentDialog Dialog_EditMovie = new ContentDialog();
            Dialog_EditMovie.XamlRoot = this.XamlRoot;
            Dialog_EditMovie.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_EditMovie.Title = "Edit Movie";
            Dialog_EditMovie.Content = new MovieInfoDialog();
            Dialog_EditMovie.DataContext = button.DataContext;
            await Dialog_EditMovie.ShowAsync();
            _viewModel.Update_FilmInfo();
        }
        private async void DeleteButton_Click(object sender, RoutedEventArgs e) {
            Button button = sender as Button;
            int movieid = (int)button.DataContext.GetType().GetProperty("MovieId").GetValue(button.DataContext);
            ContentDialog Dialog_DeleteMovie = new ContentDialog();
            Dialog_DeleteMovie.XamlRoot = this.XamlRoot;
            Dialog_DeleteMovie.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_DeleteMovie.Title = "Confirm Delete Movie";
            Dialog_DeleteMovie.PrimaryButtonText = "Yes";
            Dialog_DeleteMovie.CloseButtonText = "Cancel";
            Dialog_DeleteMovie.DefaultButton = ContentDialogButton.Primary;
            Dialog_DeleteMovie.Content = new ConfirmDeleteDialog();
            var result = await Dialog_DeleteMovie.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var deleteMovie = _context.Movies.FirstOrDefault(m => m.MovieId == movieid);
                if (deleteMovie != null)
                {
                    _context.Movies.Remove(deleteMovie);
                    _context.SaveChanges();
                }
                _viewModel.Update_FilmInfo();
            }
        }
    }
}
