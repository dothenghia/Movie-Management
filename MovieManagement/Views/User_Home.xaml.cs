using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using MovieManagement.Views;
using System.Diagnostics;

namespace MovieManagement.Views
{

    // Fake Movie Class
    public class MovieCard
    {
        public string Index { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public MovieCard(string index, string name, string image) { 
            this.Index = index;
            this.Name = name;
            this.Image = image;
        }
    }


    public sealed partial class User_Home : Page
    {

        public ObservableCollection<MovieCard> MovieCards { get; } = new ObservableCollection<MovieCard>();
        private DispatcherTimer holdTimer;

        public User_Home()
        {
            this.InitializeComponent();

            for (int i = 1; i < 10; i++)
            {
                MovieCards.Add(new MovieCard(i.ToString(), $"Movie {i.ToString()}", "ms-appx:///Assets/thumbnail-ngang.jpg"));
            }

            Blockbuster_Slider.ItemsSource = MovieCards;
            Primetime_Slider.ItemsSource = MovieCards;
            Nighttime_Slider.ItemsSource = MovieCards;
            Standardtime_Slider.ItemsSource = MovieCards;
            holdTimer = new DispatcherTimer();
            holdTimer.Interval = TimeSpan.FromSeconds(3);
        }

        private void MovieCard_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            // Get postion of sender element relative to the window
            Flyout flyout = FlyoutBase.GetAttachedFlyout(sender as FrameworkElement) as Flyout;
            flyout.ShowAt(sender as FrameworkElement);
        }

        private void MovieCard_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            
        }
    }
}
