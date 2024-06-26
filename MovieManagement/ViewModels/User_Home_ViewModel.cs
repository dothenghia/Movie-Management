﻿using MovieManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MovieManagement.ViewModels
{
    public class User_Home_ViewModel:ViewModelBase
    {
        // Get database context
        private readonly DB_MovieManagementContext _context = new DB_MovieManagementContext();

        // Create collections for movie
        public ObservableCollection<dynamic> Blockbuster_Movies { get; set; } = new ObservableCollection<dynamic>();
        public ObservableCollection<dynamic> GoldenHour_Movies { get; set; } = new ObservableCollection<dynamic>();
        public ObservableCollection<dynamic> DayTime_Movies { get; set; } = new ObservableCollection<dynamic>();
        public ObservableCollection<dynamic> NightTime_Movies { get; set; } = new ObservableCollection<dynamic>();
        public ObservableCollection<dynamic> All_Movies { get; set; } = new ObservableCollection<dynamic>();

        public User_Home_ViewModel()
        {
            // Execute query to get movies
            var allMovies = (from m in _context.Movies join a in _context.AgeCertificates on m.AgeCertificateId equals a.AgeCertificateId
                            select new
                            {
                                MovieId = m.MovieId,
                                Title = m.Title,
                                Duration = m.Duration,
                                PublishYear = m.PublishYear,
                                ImdbScore = m.ImdbScore,
                                AgeCertificateContent = a.DisplayContent,
                                AgeBackground = a.BackgroundColor,
                                AgeForeground = a.ForegroundColor,
                                PosterUrl = m.PosterUrl,
                                TrailerUrl = m.TrailerUrl,
                                Description = m.Description,
                                IsBlockbuster = m.IsBlockbuster,
                                IsGoldenHour = m.IsGoldenHour
                            }).ToList();

            // Add movies to collections
            int i = 1;
            foreach (var movie in allMovies)
            {
                // Add to Blockbuster_Movies
                if (i < 10 && (bool)movie.IsBlockbuster) {
                    Blockbuster_Movies.Add(new { 
                        Index = i++,
                        MovieId = movie.MovieId,
                        Title = movie.Title, 
                        Duration = movie.Duration + "m", 
                        PublishYear = movie.PublishYear, 
                        ImdbScore = movie.ImdbScore, 
                        AgeCertificateContent = movie.AgeCertificateContent, 
                        PosterUrl = movie.PosterUrl, 
                        TrailerUrl = movie.TrailerUrl, 
                        Description = movie.Description,
                        AgeBackground = movie.AgeBackground,
                        AgeForeground = movie.AgeForeground
                    });
                }

                // Add to GoldenHour_Movies
                if ((bool)movie.IsGoldenHour) {
                    GoldenHour_Movies.Add(new { 
                        MovieId = movie.MovieId,
                        Title = movie.Title, 
                        Duration = movie.Duration + "m", 
                        PublishYear = movie.PublishYear, 
                        ImdbScore = movie.ImdbScore, 
                        AgeCertificateContent = movie.AgeCertificateContent, 
                        PosterUrl = movie.PosterUrl, 
                        TrailerUrl = movie.TrailerUrl, 
                        Description = movie.Description,
                        AgeBackground = movie.AgeBackground,
                        AgeForeground = movie.AgeForeground
                    });
                }

                // Add to All_Movies
                All_Movies.Add(new
                {
                    MovieId = movie.MovieId,
                    Title = movie.Title, 
                    Duration = movie.Duration + "m", 
                    PublishYear = movie.PublishYear, 
                    ImdbScore = movie.ImdbScore, 
                    AgeCertificateContent = movie.AgeCertificateContent, 
                    PosterUrl = movie.PosterUrl, 
                    TrailerUrl = movie.TrailerUrl, 
                    Description = movie.Description,
                    AgeBackground = movie.AgeBackground,
                    AgeForeground = movie.AgeForeground
                });
            }


            var daytimeMovies = (from m in _context.Movies
                                 join st in _context.ShowTimes on m.MovieId equals st.MovieId
                                 where st.ShowDate.Value.Hour >= 6 && st.ShowDate.Value.Hour < 18
                                 select new
                                 {
                                     MovieId = m.MovieId,
                                     Title = m.Title,
                                     Duration = m.Duration + "m",
                                     PublishYear = m.PublishYear,
                                     ImdbScore = m.ImdbScore,
                                     AgeCertificateContent = m.AgeCertificate.DisplayContent,
                                     AgeBackground = m.AgeCertificate.BackgroundColor,
                                     AgeForeground = m.AgeCertificate.ForegroundColor,
                                     PosterUrl = m.PosterUrl,
                                     TrailerUrl = m.TrailerUrl,
                                     Description = m.Description,
                                     IsBlockbuster = m.IsBlockbuster,
                                     IsGoldenHour = m.IsGoldenHour
                                 }).Distinct().ToList();

            foreach (var movie in daytimeMovies)
            {
                DayTime_Movies.Add(new
                {
                    MovieId = movie.MovieId,
                    Title = movie.Title,
                    Duration = movie.Duration,
                    PublishYear = movie.PublishYear,
                    ImdbScore = movie.ImdbScore,
                    AgeCertificateContent = movie.AgeCertificateContent,
                    AgeBackground = movie.AgeBackground,
                    AgeForeground = movie.AgeForeground,
                    PosterUrl = movie.PosterUrl,
                    TrailerUrl = movie.TrailerUrl,
                    Description = movie.Description
                });
            }

            var nightTimeMovies = (from m in _context.Movies
                                   join st in _context.ShowTimes on m.MovieId equals st.MovieId
                                   where st.ShowDate.Value.Hour >= 18 || st.ShowDate.Value.Hour < 6
                                   select new
                                   {
                                       MovieId = m.MovieId,
                                       Title = m.Title,
                                       Duration = m.Duration + "m",
                                       PublishYear = m.PublishYear,
                                       ImdbScore = m.ImdbScore,
                                       AgeCertificateContent = m.AgeCertificate.DisplayContent,
                                       AgeBackground = m.AgeCertificate.BackgroundColor,
                                       AgeForeground = m.AgeCertificate.ForegroundColor,
                                       PosterUrl = m.PosterUrl,
                                       TrailerUrl = m.TrailerUrl,
                                       Description = m.Description,
                                       IsBlockbuster = m.IsBlockbuster,
                                       IsGoldenHour = m.IsGoldenHour
                                   }).Distinct().ToList();

            foreach (var movie in nightTimeMovies)
            {
                NightTime_Movies.Add(new
                {
                    MovieId = movie.MovieId,
                    Title = movie.Title,
                    Duration = movie.Duration,
                    PublishYear = movie.PublishYear,
                    ImdbScore = movie.ImdbScore,
                    AgeCertificateContent = movie.AgeCertificateContent,
                    AgeBackground = movie.AgeBackground,
                    AgeForeground = movie.AgeForeground,
                    PosterUrl = movie.PosterUrl,
                    TrailerUrl = movie.TrailerUrl,
                    Description = movie.Description
                });
            }
        
            
        }
    }
}
