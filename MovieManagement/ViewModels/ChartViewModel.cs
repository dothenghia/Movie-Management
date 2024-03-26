using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using MovieManagement.Models;
using SkiaSharp;
using System.Collections.Generic;
using Windows.Devices.Pwm;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MovieManagement.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
namespace MovieManagement.ViewModels;

public partial class ChartViewModel : ObservableObject
{
    public DB_MovieManagementContext _context = new DB_MovieManagementContext();
    public ChartViewModel()
    {
        int currentMonth = DateTime.Now.Month;
        var dayList = (from bill in _context.Bills
                       where bill.BookingTime.Value.Month == currentMonth
                         group bill by bill.BookingTime.Value.Date into billGroup
                         select new
                         {
                             Date = billGroup.Key,
                             Total = billGroup.Sum(b => b.Total)
                         }).ToList();

        var dayRevenueValues = new List<double>();
        var dayValues = new List<string>();
        foreach (var bill in dayList)
        {
            dayRevenueValues.Add((double)bill.Total);
            dayValues.Add(bill.Date.Day.ToString());
        }

        ((LineSeries<double>)Series[0]).Values = dayRevenueValues.ToArray();
        ((Axis)XAxes[0]).Labels = dayValues.ToArray();



        var monthList = (from bill in _context.Bills
                           group bill by bill.BookingTime.Value.Date.Month into billGroup
                           select new
                           {
                               Date = billGroup.Key,
                               Total = billGroup.Sum(b => b.Total)
                           }).ToList();

        var monthRevenueValues = new List<double>();
        var monthValues = new List<string>();
        foreach (var bill in monthList)
        {
            monthRevenueValues.Add((double)bill.Total);
            monthValues.Add(bill.Date.ToString());
        }

        ((LineSeries<double>)Series1[0]).Values = monthRevenueValues.ToArray();
        ((Axis)XAxes1[0]).Labels = monthValues.ToArray();
    }
    public ISeries[] Series { get; set; } =
    {
        new LineSeries<double>
        {
            Fill = null
        }
    };
    public ISeries[] Series1 { get; set; } =
    {
        new LineSeries<double>
        {
            Fill = null
        }
    };

    public LabelVisual Title { get; set; } =
        new LabelVisual
        {
            Text = "Daily Revenue",
            TextSize = 15,
            Padding = new LiveChartsCore.Drawing.Padding(5),
            Paint = new SolidColorPaint(SKColors.White)
        };
    public LabelVisual Title1 { get; set; } =
        new LabelVisual
        {
            Text = "Monthly Revenue",
            TextSize = 15,
            Padding = new LiveChartsCore.Drawing.Padding(5),
            Paint = new SolidColorPaint(SKColors.White)
        };
    public Axis[] XAxes { get; set; } =
    {
        new Axis
        {
            Name = "Date",
            LabelsRotation = 15,
        }
    };
    public Axis[] XAxes1 { get; set; } =
    {
        new Axis
        {
            Name = "Month",
            LabelsRotation = 15,
        }
    };
}