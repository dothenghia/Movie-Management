using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MovieManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        public Admin_FilmInfo()
        {
            this.InitializeComponent();
            DataContext = new Admin_FilmInfo_ViewModel();
        }
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Add New Movie";
            dialog.PrimaryButtonText = "Save";
            dialog.SecondaryButtonText = "Don't Save";
            dialog.CloseButtonText = "Cancel";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = new ContentDialogContent();
            dialog.RequestedTheme = (VisualTreeHelper.GetParent(sender as Button) as StackPanel).ActualTheme;

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Debug.Print("User saved their work");
            }
            else if (result == ContentDialogResult.Secondary)
            {
                Debug.Print("User did not save their work");
            }
            else
            {
                Debug.Print("User cancelled the dialog");
            }
        }
        void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }
        void DeleteButton_Click(object sender, RoutedEventArgs e) { }
    }
}
