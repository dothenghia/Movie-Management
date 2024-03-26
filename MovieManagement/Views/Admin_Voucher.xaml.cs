using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MovieManagement.Models;
using MovieManagement.ViewModels;
using System;
using System.Collections.Generic;
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
    public sealed partial class Admin_Voucher : Page
    {
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        private Admin_Voucher_ViewModel viewModel;
        public Admin_Voucher()
        {
            this.InitializeComponent();
            viewModel = new Admin_Voucher_ViewModel();
            DataContext = viewModel;
            viewModel.Update_Voucher();
        }
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog Dialog_AddVoucher = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            Dialog_AddVoucher.XamlRoot = this.XamlRoot;
            Dialog_AddVoucher.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_AddVoucher.Title = "Add Voucher";
            Dialog_AddVoucher.Content = new VoucherDialog();
            Dialog_AddVoucher.RequestedTheme = (VisualTreeHelper.GetParent(sender as Button) as StackPanel).ActualTheme;
            await Dialog_AddVoucher.ShowAsync();
            viewModel.Update_Voucher();
        }
        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ContentDialog Dialog_EditVoucher = new ContentDialog();
            Dialog_EditVoucher.XamlRoot = this.XamlRoot;
            Dialog_EditVoucher.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_EditVoucher.Title = "Edit Voucher";
            Dialog_EditVoucher.Content = new VoucherDialog();
            Dialog_EditVoucher.DataContext = button.DataContext;
            await Dialog_EditVoucher.ShowAsync();
            viewModel.Update_Voucher();
        }
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int voucherId = (int)button.DataContext.GetType().GetProperty("VoucherId").GetValue(button.DataContext);
            ContentDialog Dialog_DeleteVoucher = new ContentDialog();
            Dialog_DeleteVoucher.XamlRoot = this.XamlRoot;
            Dialog_DeleteVoucher.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            Dialog_DeleteVoucher.Title = "Confirm Delete Voucher";
            Dialog_DeleteVoucher.PrimaryButtonText = "Yes";
            Dialog_DeleteVoucher.CloseButtonText = "Cancel";
            Dialog_DeleteVoucher.DefaultButton = ContentDialogButton.Primary;
            Dialog_DeleteVoucher.Content = new ConfirmDeleteDialog();
            var result = await Dialog_DeleteVoucher.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var deleteVoucher = _context.Vouchers.FirstOrDefault(v => v.VoucherId == voucherId);
                if (deleteVoucher != null)
                {
                    _context.Vouchers.Remove(deleteVoucher);
                    _context.SaveChanges();
                }
            }
            viewModel.Update_Voucher();
        }
    }
}
