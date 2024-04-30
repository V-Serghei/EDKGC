using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EDKGC.ViewModel;
using EDKGC.ViewModel.CentralSolutions;
using EDKGC.ViewModel.ISO27001;
using LiveCharts;
using LiveCharts.Wpf;

namespace EDKGC.Views.Windows
{
    public partial class ISOWindow : Window
    {
        public ObservableCollection<DataTable> ThreatDataCollection { get; set; }
        public ISOWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelLocator();

            ThreatDataCollection = new ObservableCollection<DataTable>();
            dataGrid.ItemsSource = ThreatDataCollection;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == "Low")
            {
                textBox.Background = Brushes.Yellow;
            }
            else if (textBox.Text == "Medium")
            {
                textBox.Background = Brushes.Orange;
            }
            else if(textBox.Text == "High")
            {
                textBox.Background = Brushes.Red;
            }
            else if (textBox.Text == "Critical")
            {
                textBox.Background = Brushes.DarkRed;

            }
            else
            {
                textBox.Background = Brushes.Green;
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double totalAle = 0;

            foreach (var data in ThreatDataCollection)
            {
                totalAle += data.ALE;
            }
            TextBoxResult.Text ="Результат подсчета, общие траты могут составить: $"+ totalAle.ToString(CultureInfo.InvariantCulture);
        }



        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    //if (DataContext is ISOViewModel viewModel && viewModel.IsEndQuestion())
        //    {
        //        var window = new IsoResultsWidow();
        //        window.Show();
        //    }
        //}

    }
}