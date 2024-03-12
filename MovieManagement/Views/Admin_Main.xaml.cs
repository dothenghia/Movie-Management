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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MovieManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public class TopTen
    {
        public string Title { get; set; }
        public string ImageLocation { get; set; }
        public int TicketCount { get; set; }
        // Other properties
        public TopTen(string image, string title, int count)
        {
            this.ImageLocation = image;
            this.Title = title;
            this.TicketCount = count;
        }
    }
    public sealed partial class Admin_Main : Page
    {
        public ObservableCollection<TopTen> myList { get; } = new ObservableCollection<TopTen>();
        // Other properties and methods

        public Admin_Main()
        {
            this.InitializeComponent();
            myList.Add(new TopTen("ms-appx:///Assets/thumbnail-ngang.jpg", "Dao", 1));
            myList.Add(new TopTen("ms-appx:///Assets/thumbnail-ngang.jpg", "Mai", 4));
            myList.Add(new TopTen("ms-appx:///Assets/thumbnail-ngang.jpg", "Daoeee", 5));
            myList.Add(new TopTen("ms-appx:///Assets/thumbnail-ngang.jpg", "Dao", 8));

        }
    }
}
