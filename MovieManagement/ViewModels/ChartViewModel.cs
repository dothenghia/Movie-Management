using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;

namespace MovieManagement.ViewModels;

public partial class ChartViewModel : ObservableObject
{
    public ISeries[] Series { get; set; } =
    {
        new LineSeries<double>
        {
            Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
            Fill = null
        }
    };

    public LabelVisual Title { get; set; } =
        new LabelVisual
        {
            Text = "Revenue",
            TextSize = 10,
            Padding = new LiveChartsCore.Drawing.Padding(15),
            Paint = new SolidColorPaint(SKColors.DarkSlateGray)
        };
}