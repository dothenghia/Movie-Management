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

namespace MovieManagement.Views
{
    public delegate void SignupDialogClosedEventHandler();

    public sealed partial class SignupDialog : Page
    {
        public event SignupDialogClosedEventHandler SignupValid;

        public SignupDialog()
        {
            this.InitializeComponent();
        }

        // ====== Event Click for Cancel button
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog parentDialog = this.Parent as ContentDialog;
            if (parentDialog != null) {
                parentDialog.Hide();
            }
        }

        // ====== Event Click for Signup button
        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog parentDialog = this.Parent as ContentDialog;
            if (parentDialog != null)
            {
                parentDialog.Hide();

                SignupValid?.Invoke();
            }
        }
    }
}
