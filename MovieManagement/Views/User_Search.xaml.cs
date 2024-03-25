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
using MovieManagement.ViewModels;
using CommunityToolkit.Mvvm.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MovieManagement.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class User_Search : Page
    {
        private User_Search_ViewModel viewModel;
        public User_Search()
        {
            this.InitializeComponent();
            viewModel = new User_Search_ViewModel();
            DataContext = viewModel;
            Filter.SelectedItem = Filter.Items.FirstOrDefault();
            viewModel.Update_MovieSort(Filter.Items.FirstOrDefault().ToString());
            viewModel.UpdatePagedMovies();
        }

        private void BackButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void FilterCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFilter = (sender as ComboBox).SelectedItem as string;
            viewModel.stored_sort = selectedFilter;
            viewModel.Update_Movie();
        }

        private void FilterGenreCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFilter = (sender as ComboBox).SelectedItem as string;
            if (selectedFilter == "All")
                viewModel.stored_genre = "";
            else viewModel.stored_genre = selectedFilter;
            viewModel.Update_Movie();
        }

        private void FilterYearCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFilter = (sender as ComboBox).SelectedItem as string;
            if (selectedFilter == "All")
                viewModel.stored_year = "";
            else viewModel.stored_year = selectedFilter;
            viewModel.Update_Movie();
        }

        private void FilterAgeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFilter  = (sender as ComboBox).SelectedItem as string;
            if (selectedFilter == "All")
                viewModel.stored_agecertification = "";
            else viewModel.stored_agecertification = selectedFilter;
            viewModel.Update_Movie();
        }

    }
}
