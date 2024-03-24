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
using System.Data.SqlTypes;
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
    public sealed partial class User_ExportTicket : Page
    {
        public User_ExportTicket()
        {
            this.InitializeComponent();
            DataContext = new ViewModels.User_ExportTicket_ViewModel();
        }

        private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            var selected = sender as ToggleButton;
            string substring = selected.Content.ToString();
            if (selected.IsChecked == true && !GlobalContext.seats.Contains(substring))
            {
                GlobalContext.setVoucher(substring);
            }
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            var unselectedToggleButton = sender as ToggleButton;
            string substring = unselectedToggleButton.Content.ToString();

            if (GlobalContext.voucher.Contains(substring))
            {
                GlobalContext.removeVoucher(substring);
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is User_ExportTicket_ViewModel viewModel)
            {
                viewModel.ApplyCommand.Execute(this);
                ApplyButton.IsEnabled = false;
            }
        }

        private async void ConfirmButton_Click(Object sender, RoutedEventArgs e)
        {
            if (DataContext is User_ExportTicket_ViewModel viewModel)
            {
                viewModel.ConfirmCommand.Execute(this);
                ContentDialog contentDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    Title = "Success",
                    Content = "You booking succesfully!",
                    CloseButtonText = "Ok"
                };
                await contentDialog.ShowAsync();
            }
        }
    }
}
