using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using MovieManagement.ViewModels;
using MovieManagement.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MovieManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Admin_FilmCerti : Page
    {
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public Admin_FilmCerti()
        {
            this.InitializeComponent();
            DataContext = new Admin_FilmCerti_ViewModel();
        }
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog Dialog_AddNewCerti = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            Dialog_AddNewCerti.XamlRoot = this.XamlRoot;
            Dialog_AddNewCerti.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_AddNewCerti.Title = "Add New Certificate";
            Dialog_AddNewCerti.Content = new MovieCertiDialog();
            Dialog_AddNewCerti.RequestedTheme = (VisualTreeHelper.GetParent(sender as Button) as StackPanel).ActualTheme;
            await Dialog_AddNewCerti.ShowAsync();
        }
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ContentDialog Dialog_EditCerti = new ContentDialog();
            Dialog_EditCerti.XamlRoot = this.XamlRoot;
            Dialog_EditCerti.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_EditCerti.Title = "Edit Certificate Information";
            Dialog_EditCerti.Content = new MovieCertiDialog();
            Dialog_EditCerti.DataContext = button.DataContext;
            await Dialog_EditCerti.ShowAsync();
        }
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int certificateId = (int)button.DataContext.GetType().GetProperty("AgeCertificateId").GetValue(button.DataContext);
            ContentDialog Dialog_DeleteCerti = new ContentDialog();
            Dialog_DeleteCerti.XamlRoot = this.XamlRoot;
            Dialog_DeleteCerti.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_DeleteCerti.Title = "Confirm Delete Certificate";
            Dialog_DeleteCerti.PrimaryButtonText = "Yes";
            Dialog_DeleteCerti.CloseButtonText = "Cancel";
            Dialog_DeleteCerti.DefaultButton = ContentDialogButton.Primary;
            Dialog_DeleteCerti.Content = new ConfirmDeleteDialog();
            var result = await Dialog_DeleteCerti.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var deleteCerti = _context.AgeCertificates.FirstOrDefault(a => a.AgeCertificateId == certificateId);
                if (deleteCerti != null)
                {
                    _context.AgeCertificates.Remove(deleteCerti);
                    _context.SaveChanges();
                }
            }
        }
    }
}
