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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using MovieManagement.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MovieManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Admin_ShowTime : Page
    {
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        private Admin_ShowTime_ViewModel _viewModel;
        public Admin_ShowTime()
        {
            this.InitializeComponent();
            _viewModel = new Admin_ShowTime_ViewModel();
            DataContext = _viewModel;
            _viewModel.Update_Showtimes();
        }
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog Dialog_AddShowtime = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            Dialog_AddShowtime.XamlRoot = this.XamlRoot;
            Dialog_AddShowtime.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_AddShowtime.Title = "Add Showtime";
            Dialog_AddShowtime.Content = new ShowtimeDialog();
            Dialog_AddShowtime.RequestedTheme = (VisualTreeHelper.GetParent(sender as Button) as StackPanel).ActualTheme;
            await Dialog_AddShowtime.ShowAsync();
            _viewModel.Update_Showtimes();
        }
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ContentDialog Dialog_EditShowtime = new ContentDialog();
            Dialog_EditShowtime.XamlRoot = this.XamlRoot;
            Dialog_EditShowtime.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_EditShowtime.Title = "Edit Showtime";
            Dialog_EditShowtime.Content = new ShowtimeDialog();
            Dialog_EditShowtime.DataContext = button.DataContext;
            await Dialog_EditShowtime.ShowAsync();
            _viewModel.Update_Showtimes();
        }
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int showtimeId = (int)button.DataContext.GetType().GetProperty("ShowTimeId").GetValue(button.DataContext);
            ContentDialog Dialog_DeleteShowtime = new ContentDialog();
            Dialog_DeleteShowtime.XamlRoot = this.XamlRoot;
            Dialog_DeleteShowtime.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_DeleteShowtime.Title = "Confirm Delete Showtime";
            Dialog_DeleteShowtime.PrimaryButtonText = "Yes";
            Dialog_DeleteShowtime.CloseButtonText = "Cancel";
            Dialog_DeleteShowtime.DefaultButton = ContentDialogButton.Primary;
            Dialog_DeleteShowtime.Content = new ConfirmDeleteDialog();
            var result = await Dialog_DeleteShowtime.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var deleteShowtime = _context.ShowTimes.FirstOrDefault(s => s.ShowTimeId == showtimeId);
                if (deleteShowtime != null)
                {
                    _context.ShowTimes.Remove(deleteShowtime);
                    _context.SaveChanges();
                }
                _viewModel.Update_Showtimes();
            }
        }
    }
}
