using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MovieManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MovieCertiDialog : Page
    {
        bool isAdd = false;
        bool isEdit = false;
        string background ="", foreground = "";
        private DB_MovieManagementContext _context = new DB_MovieManagementContext();
        public MovieCertiDialog()
        {
            this.InitializeComponent();
        }
        private void BackgroundColorButton_Click(Microsoft.UI.Xaml.Controls.SplitButton sender, Microsoft.UI.Xaml.Controls.SplitButtonClickEventArgs args)
        {
            var border = (Border)BackgroundColorButton.Content;
            var color = ((SolidColorBrush)border.Background).Color;

            SolidColorBrush BackgroundColor = new SolidColorBrush(color);
            BackgroundAC.Background = BackgroundColor;
        }
        private void ForegroundColorButton_Click(Microsoft.UI.Xaml.Controls.SplitButton sender, Microsoft.UI.Xaml.Controls.SplitButtonClickEventArgs args)
        {
            var border = (Border)BackgroundColorButton.Content;
            var color = ((SolidColorBrush)border.Background).Color;

            SolidColorBrush ForegroundColor = new SolidColorBrush(color);
            ForegroundAC.Foreground = ForegroundColor;
        }
        private void GridView1_ItemClick(object sender, ItemClickEventArgs e)
        {
            var rect = (Rectangle)e.ClickedItem;
            background = AutomationProperties.GetName(rect);
            var color = ((SolidColorBrush)rect.Fill).Color;
            SolidColorBrush BackgroundColor = new SolidColorBrush(color);
            BackgroundAC.Background = BackgroundColor;
            CurrentBackgroundColor.Background = new SolidColorBrush(color);
            Task.Delay(10).ContinueWith(_ => BackgroundColorButton.Flyout.Hide(), TaskScheduler.FromCurrentSynchronizationContext());
        }
        private void GridView2_ItemClick(object sender, ItemClickEventArgs e)
        {
            var rect = (Rectangle)e.ClickedItem;
            foreground = AutomationProperties.GetName(rect);
            var color = ((SolidColorBrush)rect.Fill).Color;
            SolidColorBrush ForegroundColor = new SolidColorBrush(color);
            ForegroundAC.Foreground = ForegroundColor;
            CurrentForegroundColor.Background = new SolidColorBrush(color);
            Task.Delay(10).ContinueWith(_ => ForegroundColorButton.Flyout.Hide(), TaskScheduler.FromCurrentSynchronizationContext());
        }

        //check mode add/edit
        private void DialogPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(InvisibleTextBlock.Text)) { isAdd = true; isEdit = false; }
            else { isAdd = false; isEdit = true; background = AutomationProperties.GetName(CurrentBackgroundColor); foreground = AutomationProperties.GetName(CurrentForegroundColor); }
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
            int certiid;
            int.TryParse(InvisibleTextBlock.Text, out certiid);
            string certiname = CertificateName_TextBox.Text;
            string requireageText = RequireAge_TextBox.Text;
            int requireage;
            int.TryParse(RequireAge_TextBox.Text, out requireage);

            /////check certificate name
            if (certiname == null || certiname.Length == 0)
            {
                MessageBox.Text = "Certificate name must not be null.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(CertificateName_TextBox) as Flyout;
                flyout.ShowAt(CertificateName_TextBox);
                return;
            }

            Regex regexname = new Regex(@"^[A-Za-z0-9\-+/ ]*$");
            if (!regexname.IsMatch(certiname))
            {
                MessageBox.Text = "Certificate name must contain letters, digits,\nand may include '-', '+', '/', and space characters only.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(CertificateName_TextBox) as Flyout;
                flyout.ShowAt(CertificateName_TextBox);
                return;
            }

            var certi = _context.AgeCertificates.Where(a => a.DisplayContent == certiname && a.AgeCertificateId != certiid).FirstOrDefault();
            if (certi != null)
            {
                MessageBox.Text = "This certificate already exists.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(CertificateName_TextBox) as Flyout;
                flyout.ShowAt(CertificateName_TextBox);
                return;
            }

            Debug.Print("BG+FG="+ background + foreground);
            if (background == "")
            {
                MessageBox.Text = "Please select a background color.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(BackgroundColorButton) as Flyout;
                flyout.ShowAt(BackgroundColorButton);
                return;
            }
            if (foreground == "")
            {
                MessageBox.Text = "Please select a foreground color.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(ForegroundColorButton) as Flyout;
                flyout.ShowAt(ForegroundColorButton);
                return;
            }

            /////check color
            if (background == foreground)
            {
                MessageBox.Text = "Background and foreground must not be same color";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(BackgroundAC) as Flyout;
                flyout.ShowAt(BackgroundAC);
                return;
            }
            var certibg = _context.AgeCertificates.Where(a => a.BackgroundColor == background && a.ForegroundColor == foreground).FirstOrDefault();
            if (certibg != null && isAdd)
            {
                MessageBox.Text = "Already have a certificate with this style.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(BackgroundAC) as Flyout;
                flyout.ShowAt(BackgroundAC);
                return;
            }

            /////check require age
            if (requireageText == null || requireageText.Length == 0)
            {
                MessageBox.Text = "Require age must not be null.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(RequireAge_TextBox) as Flyout;
                flyout.ShowAt(RequireAge_TextBox);
                return;
            }

            Regex regexage = new Regex(@"\b(?:0|[1-9]\d?|100)\b");
            if (!regexage.IsMatch(requireageText))
            {
                MessageBox.Text = "Require age must be a number between 0 and 100.";
                Flyout flyout = FlyoutBase.GetAttachedFlyout(CertificateName_TextBox) as Flyout;
                flyout.ShowAt(RequireAge_TextBox);
                return;
            }


            /////CE command
            if (isAdd)
            {
                AgeCertificate newCerti = new AgeCertificate
                {
                    DisplayContent = certiname,
                    RequireAge = requireage,
                    BackgroundColor = background,
                    ForegroundColor = foreground
                };
                _context.AgeCertificates.Add(newCerti);
                _context.SaveChanges();
            }
            else if (isEdit)
            {
                var editcerti = _context.AgeCertificates.Where(a => a.AgeCertificateId == certiid).FirstOrDefault();
                if (editcerti != null)
                {
                    editcerti.DisplayContent = certiname;
                    editcerti.RequireAge = requireage;
                    editcerti.BackgroundColor = background;
                    editcerti.ForegroundColor = foreground;
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
