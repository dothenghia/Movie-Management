using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MovieManagement.Converters;
using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MovieManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VoucherDialog : Page
    {
        bool isAdd = false;
        bool isEdit = false;
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public VoucherDialog()
        {
            this.InitializeComponent();
        }
        private void DialogPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(InvisibleTextBlock.Text)) { isAdd = true; isEdit = false; }
            else { isAdd = false; isEdit = true; }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog parentDialog = this.Parent as ContentDialog;
            if (parentDialog != null)
            {
                parentDialog.Hide();
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int voucherid;
            int.TryParse(InvisibleTextBlock.Text, out voucherid);
            
            // Validate voucher code
            string vouchercode = VoucherCode_TextBox.Text;
            if (vouchercode == null || vouchercode.Length == 0)
            {
                MessageBox.Text = "Voucher code must not be null.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(VoucherCode_TextBox) as Flyout;
                flyout.ShowAt(VoucherCode_TextBox);
                return;
            }
            Regex regex = new Regex(@"^[A-Z0-9]*$");
            if (!regex.IsMatch(vouchercode))
            {
                MessageBox.Text = "Voucher code must contains capital letters and digits only.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(VoucherCode_TextBox) as Flyout;
                flyout.ShowAt(VoucherCode_TextBox);
                return;
            }
            var voucher = _context.Vouchers.Where(g => g.VoucherCode == vouchercode).FirstOrDefault();
            if (isAdd && voucher != null)
            {
                MessageBox.Text = "This voucher code already exists.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(VoucherCode_TextBox) as Flyout;
                flyout.ShowAt(VoucherCode_TextBox);
                return;
            }

            // Validate status
            var selectedstatus = Status_ComboBox.SelectedItem;
            if (selectedstatus == null) {
                MessageBox.Text = "Please select a status.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Status_ComboBox) as Flyout;
                flyout.ShowAt(Status_ComboBox);
                return;
            }
            var statusconverter = new ExpiredStatusConverter();
            bool isExpired = (bool)statusconverter.ConvertBack(selectedstatus, typeof(bool), null, null);

            var selectedtype = Type_ComboBox.SelectedItem;
            if (selectedtype == null)
            {
                MessageBox.Text = "Please select a discount type.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(Type_ComboBox) as Flyout;
                flyout.ShowAt(Type_ComboBox);
                return;
            }
            var typeconverter = new DiscountTypeConverter();
            bool isPercentageDiscount = (bool)typeconverter.ConvertBack(selectedtype, typeof(bool), null, null);

            //Validate discount amount
            string discountamount = DiscountAmount_TextBox.Text;
            if (discountamount == null || discountamount.Length == 0)
            {
                MessageBox.Text = "Please enter discount amount.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(DiscountAmount_TextBox) as Flyout;
                flyout.ShowAt(DiscountAmount_TextBox);
                return;
            }
            Regex regexdouble = new Regex(@"^[0-9]*\.?[0-9]*$");
            Regex regexpercentage = new Regex(@"^0\.[0-9]*$");
            if (!regexdouble.IsMatch(discountamount))
            {
                MessageBox.Text = "Discount amount must be a number.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(DiscountAmount_TextBox) as Flyout;
                flyout.ShowAt(DiscountAmount_TextBox);
                return;
            }
            if (isPercentageDiscount)
            {
                if (!regexpercentage.IsMatch(discountamount))
                {
                    MessageBox.Text = "Percentage discount amount must be a number between 0 and 1.";
                    Flyout flyout = FlyoutBase.GetAttachedFlyout(DiscountAmount_TextBox) as Flyout;
                    flyout.ShowAt(DiscountAmount_TextBox);
                    return;
                }
            }
            double dcam;
            double.TryParse(discountamount, NumberStyles.Float, CultureInfo.InvariantCulture, out dcam);

            //Validate requirement amount
            string requirementamount = RequirementAmount_TextBox.Text;
            if (requirementamount == null || requirementamount.Length == 0)
            {
                MessageBox.Text = "Please enter requirement amount.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(RequirementAmount_TextBox) as Flyout;
                flyout.ShowAt(RequirementAmount_TextBox);
                return;
            }
            if (!regexdouble.IsMatch(requirementamount))
            {
                MessageBox.Text = "Requirement amount must be a number.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(RequirementAmount_TextBox) as Flyout;
                flyout.ShowAt(RequirementAmount_TextBox);
                return;
            }
            double rqam;
            double.TryParse(requirementamount, NumberStyles.Float, CultureInfo.InvariantCulture, out rqam);

            if (isAdd)
            {
                Voucher newVoucher = new Voucher
                {
                    VoucherCode = vouchercode,
                    IsExpired = isExpired,
                    IsPercentageDiscount = isPercentageDiscount,
                    DiscountAmount = dcam,
                    RequirementAmount = rqam
                };
                _context.Vouchers.Add(newVoucher);
                _context.SaveChanges();
            }
            else if (isEdit)
            {
                var editvoucher = _context.Vouchers.Where(v => v.VoucherId == voucherid).FirstOrDefault();
                if (editvoucher != null)
                {
                    editvoucher.VoucherCode = vouchercode;
                    editvoucher.IsExpired = isExpired;
                    editvoucher.IsPercentageDiscount = isPercentageDiscount;
                    editvoucher.DiscountAmount = dcam;
                    editvoucher.RequirementAmount = rqam;
                    _context.SaveChanges();
                }
            }

            // -- Close Dialog and Raise SignupValid event
            ContentDialog parentDialog = this.Parent as ContentDialog;
            if (parentDialog != null)
            {
                parentDialog.Hide();
            }
        }
    }
}
