using ABI.Windows.ApplicationModel.Activation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using MovieManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace MovieManagement.Views
{
    // Temp Class Data
    public class Showtime
    {
        public int ShowTimeId { get; set; }
        public int MovieId { get; set; }
        public DateTime ShowDate { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public Showtime(int showtimeid, int movieid, DateTime showdate)
        {
            this.ShowTimeId = showtimeid;
            this.MovieId = movieid;
            this.ShowDate = showdate;
        }
    }

    // Temp Class Data
    public class Ticket
    {
        public int id { get; set; }
        public string position { get; set; }
        public bool isAvailable { get; set; }

        public Ticket(int id, string row, string column, bool isAvailable = true)
        {
            this.id = id;
            this.position = row + column;
            this.isAvailable = isAvailable;
        }
    }

    public sealed partial class User_Movie : Page
    {
        public int MovieID;
        public FrameworkElement ParentFrame;

        public ObservableCollection<Showtime> Showtimes { get; } = new ObservableCollection<Showtime>();
        public ObservableCollection<Ticket> Tickets { get; } = new ObservableCollection<Ticket>();
        public ObservableCollection<String> daysOfWeek { get; } = new ObservableCollection<String>();
        public ObservableCollection<String> timesOfDay { get; } = new ObservableCollection<String>();

        public User_Movie()
        {
            this.InitializeComponent();

            // ############# INFORMATION TAB #############




            // ############# SHOWTIME TAB #############
            this.SizeChanged += OnWindowSizeChanged;
            DateTime currentDate = DateTime.Today;
            for (int i = 0; i < 7; i++)
            {
                daysOfWeek.Add(currentDate.AddDays(i).ToString("dd/MM/yyyy"));
            }

            for (int i = 0; i < 7; i++)
            {
                Showtimes.Add(new Showtime(i, i, new DateTime(2024, 3, 18 + i, 19, 00, 15)));
                Showtimes.Add(new Showtime(i, i, new DateTime(2024, 3, 18 + i, 09, 15, 15)));
                Showtimes.Add(new Showtime(i, i, new DateTime(2024, 3, 18 + i, 17, 25, 15)));
                Showtimes.Add(new Showtime(i, i, new DateTime(2024, 3, 18 + i, 19, 40, 15)));
                Showtimes.Add(new Showtime(i, i, new DateTime(2024, 3, 18 + i, 13, 55, 15)));
                Showtimes.Add(new Showtime(i, i, new DateTime(2024, 3, 18 + i, 16, 35, 15)));
            }
            //DateGridView.ItemsSource = daysOfWeek;
            for (char j = 'A'; j <= 'C'; j++)
                for (int i = 0; i < 10; i++)
                {
                    Tickets.Add(new Ticket(i, j.ToString(), i.ToString(), false));
                }
            for (char j = 'D'; j <= 'G'; j++)
                for (int i = 10; i < 20; i++)
                {
                    Tickets.Add(new Ticket(i, j.ToString(), i.ToString(), true));
                }
        }

        // ####################### INFORMATION TAB #######################
        // ========== Get attached data when Navigate from User_Home.xaml
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter != null) {
                MovieID = (int)e.Parameter;
            }
        }

        // ========== Binding Context to UI
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new User_Movie_ViewModel(MovieID);
        }

        // ========== Event PointerEntered for BioFlyout
        private void Contributor_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.ParentFrame = sender as FrameworkElement;
            (ParentFrame as Border).Opacity = 0.6;
        }

        // ========== Event PointerExited for BioFlyout
        private void Contributor_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            (ParentFrame as Border).Opacity = 1;
        }

        // ========== Event Tapped for BioFlyout
        private void Contributor_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Flyout flyout = FlyoutBase.GetAttachedFlyout(sender as FrameworkElement) as Flyout;
            flyout.ShowAt(sender as FrameworkElement);
        }

        // -- Event Click for Back Button
        private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void ShowTimeButton_Click(object sender, RoutedEventArgs e)
        {
            int showTimePivotIndex = 1;
            MainPivot.SelectedIndex = showTimePivotIndex;
        }



        // ###################### SHOWTIME TAB ######################
        // -- Event Window Size Changed
        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs eventArgs)
        {
            double newWidth = eventArgs.NewSize.Width;
            BezierSegment curve = ScreenLine;
            curve.Point1 = new Point(newWidth / 3, 10);
            curve.Point2 = new Point(newWidth / 3 * 2, 10);
            curve.Point3 = new Point(newWidth, 50);
        }


        // -- Event Click for DateGridView
        private void DateGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickItem = e.ClickedItem ;
            if (clickItem != null&& DataContext is User_Movie_ViewModel viewModel)
            {
                viewModel.DateSelectionCommand.Execute(clickItem.ToString());
            }
            //SeatsSelectionMap.Visibility = Visibility.Collapsed;
        }


        // -- Event Click for TimeGridView
        private void ShowTimes_ItemClick(object sender, ItemClickEventArgs e)
        {
            SeatsSelectionMap.Visibility = Visibility.Visible;
            seats = "";
            TextBlock textBlock = SeatsSelection;
            SeatsSelection.Text = "";
            var clickItem = e.ClickedItem;
            if (clickItem != null &&  DataContext is User_Movie_ViewModel viewModel)
            {
                viewModel.ShowtimeSelectionCommand.Execute(clickItem.ToString());
            }
        }
        string seats = "";


        // -- Event Click for ToggleButton
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            var selectedToggleButton = sender as ToggleButton;
            TextBlock textBlock = SeatsSelection;
            if (selectedToggleButton != null)
            {
                seats += selectedToggleButton.Content.ToString() + " ";
                SeatsSelection.Text = "Your selection: " + seats;
            }
        }





    }
}
