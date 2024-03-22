using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
//using MovieManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// 
    public sealed partial class Admin_Main : Page
    {
        
        // Other properties and methods

        public Admin_Main()
        {
            this.InitializeComponent();
            DataContext = new Admin_Main_ViewModel();
        }
        private void ShowtimeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is string selectedItem)
            {
                Binding binding = new Binding();  
                binding.Source = this.DataContext;  

                if (selectedItem == "Daily")
                {
                    binding.Path = new PropertyPath("showtimesDaily"); 
                }
                else if (selectedItem == "Weekly")
                {
                    binding.Path = new PropertyPath("showtimesWeekly");  
                }
                else if (selectedItem == "Monthly")
                {
                    binding.Path = new PropertyPath("showtimesMonthly");
                }

                showtimesCountDisplay.SetBinding(TextBlock.TextProperty, binding);  
            }
        }


    }
}
