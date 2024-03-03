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


namespace MovieManagement
{

    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // Load default page
            MainContent.Navigate(typeof(Views.Home));
        }

        // Navigate to the page corresponding to selected item
        private void NavigationBar_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem item = args.SelectedItem as NavigationViewItem;
            if (item != null)
            {
                Type pageType = typeof(Views.Home); // default page type

                if (item.Tag.ToString() == "Page1_Tag")
                {
                    pageType = typeof(Views.BlankPage1);
                }
                else if (item.Tag.ToString() == "Page2_Tag")
                {
                    pageType = typeof(Views.BlankPage2);
                }

                MainContent.Navigate(pageType);
            }
        }




    }
}
