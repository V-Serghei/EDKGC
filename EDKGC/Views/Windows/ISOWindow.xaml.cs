using System.Windows;
using System.Windows.Media;
using EDKGC.ViewModel;
using LiveCharts;
using LiveCharts.Wpf;

namespace EDKGC.Views.Windows
{
    public partial class ISOWindow : Window
    {
        public ISOWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelLocator();

            SeriesCollection seriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Серия 1",
                    Values = new ChartValues<double> { 5 },
                    Fill = System.Windows.Media.Brushes.Blue
                },
                new PieSeries
                {
                    Title = "Серия 2",
                    Values = new ChartValues<double> { 3 },
                    Fill = System.Windows.Media.Brushes.Red
                }
            };

            // Set the data series to the PieChart
            PieChart.Series = seriesCollection;
        }

    }
}