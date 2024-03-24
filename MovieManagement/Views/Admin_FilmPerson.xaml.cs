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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MovieManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Admin_FilmPerson : Page
    {
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public Admin_FilmPerson()
        {
            this.InitializeComponent();
            DataContext = new Admin_FilmPerson_ViewModel();
        }
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog Dialog_AddPerson = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            Dialog_AddPerson.XamlRoot = this.XamlRoot;
            Dialog_AddPerson.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_AddPerson.Title = "Add New Person";
            Dialog_AddPerson.Content = new MovieDirectorDialog();
            Dialog_AddPerson.RequestedTheme = (VisualTreeHelper.GetParent(sender as Button) as StackPanel).ActualTheme;
            await Dialog_AddPerson.ShowAsync();
        }
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ContentDialog Dialog_EditPerson = new ContentDialog();
            Dialog_EditPerson.XamlRoot = this.XamlRoot;
            Dialog_EditPerson.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_EditPerson.Title = "Edit Person Infomation";
            Dialog_EditPerson.Content = new MovieDirectorDialog();
            Dialog_EditPerson.DataContext = button.DataContext;
            await Dialog_EditPerson.ShowAsync();

        }
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int personId = (int)button.DataContext.GetType().GetProperty("PersonId").GetValue(button.DataContext);
            ContentDialog Dialog_DeletePerson = new ContentDialog();
            Dialog_DeletePerson.XamlRoot = this.XamlRoot;
            Dialog_DeletePerson.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_DeletePerson.Title = "Confirm Delete Person";
            Dialog_DeletePerson.PrimaryButtonText = "Yes";
            Dialog_DeletePerson.CloseButtonText = "Cancel";
            Dialog_DeletePerson.DefaultButton = ContentDialogButton.Primary;
            Dialog_DeletePerson.Content = new ConfirmDeleteDialog();
            var result = await Dialog_DeletePerson.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var deletePerson = _context.People.FirstOrDefault(p => p.PersonId == personId);
                if (deletePerson != null)
                {
                    _context.People.Remove(deletePerson);
                    _context.SaveChanges();
                }
            }
        }
    }
}
