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
        }

        private void MovieCard_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var border = sender as Border;
            var movie = border?.DataContext as MovieCard;

            if (movie != null)
            {
                string movieInfo = $"Name: {movie.Name}";
                Debug.WriteLine(movieInfo);
            }

            // Navigate to User_Movie page
            this.Frame.Navigate(typeof(User_Movie), movie.Name);
        }
    }
}
