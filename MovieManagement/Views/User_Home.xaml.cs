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
using MovieManagement.ViewModels;

namespace MovieManagement.Views
{
    public sealed partial class User_Home : Page
    {
        // -- Setting for Highlight Trailer
        private DispatcherTimer hoverTimer;
        private Flyout highLightCard;
        private FrameworkElement parentFrame;
        private MediaPlayerElement trailerMediaPlayer;

        public User_Home()
        {
            this.InitializeComponent();


            // ========== Binding Context to UI
            DataContext = new User_Home_ViewModel();



            // ========== Set Hold timer for Movie Card
            hoverTimer = new DispatcherTimer();
            hoverTimer.Interval = TimeSpan.FromSeconds(1); // Wait 1 second before showing the highlight card
            hoverTimer.Tick += HoverTimerConfig;
        }


        // -- Navigate to Movie Detail Page
        private void DetailMovie_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement button)
            {
                var movieId = (button.DataContext as dynamic)?.MovieId; // ------ Get MovieId
                //Debug.WriteLine((int)movieId);

                if (movieId != null)
                {
                    Frame.Navigate(typeof(User_Movie), movieId);
                }
            }
        }


        // -- Config Highlight Card Position
        private void HoverTimerConfig(object sender, object e)
        {
            hoverTimer.Stop();
            FlyoutShowOptions coordinates = new FlyoutShowOptions();
            double parentHeight = parentFrame.ActualHeight;
            coordinates.Position = new Point(220, parentHeight + 60); // Center -> (GridCard / 2 , GridCard + 60)
            coordinates.ShowMode = FlyoutShowMode.Transient;
            highLightCard.ShowAt(parentFrame, coordinates);
        }

        // -- Event PointerEntered for Movie Card
        private void MovieCard_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            hoverTimer.Start();
            this.highLightCard = FlyoutBase.GetAttachedFlyout(sender as FrameworkElement) as Flyout;
            this.parentFrame = sender as FrameworkElement;
            (parentFrame as Grid).Opacity = 0.7;
        }

        // -- Event PointerExited for Movie Card
        private void MovieCard_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            hoverTimer.Stop();
            (parentFrame as Grid).Opacity = 1;
        }

        // -- Event Closing for Highlight Card
        private void HighLightCard_Closing(object sender, object e)
        {
            Grid content = (sender as Flyout).Content as Grid;
            trailerMediaPlayer = content.FindName("Trailer_MediaPlayerElement") as MediaPlayerElement;
            if (trailerMediaPlayer != null)
            {
                trailerMediaPlayer.MediaPlayer.Pause();
            }
            (parentFrame as Grid).Opacity = 1;
        }

        // -- Event Opened for Highlight Card
        private void HighLightCard_Opened(object sender, object e)
        {
            Grid highlightContent = highLightCard.Content as Grid;

            trailerMediaPlayer = highlightContent.FindName("Trailer_MediaPlayerElement") as MediaPlayerElement;
            if (trailerMediaPlayer != null)
            {
                trailerMediaPlayer.MediaPlayer.Play();
                trailerMediaPlayer.MediaPlayer.Position = TimeSpan.FromSeconds(0);
                trailerMediaPlayer.MediaPlayer.IsLoopingEnabled = true;
            }
        }
    }
}
