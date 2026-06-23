using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
            ThreatDataCollection.CollectionChanged += ThreatDataCollection_CollectionChanged;
            dataGrid.ItemsSource = ThreatDataCollection;
            UpdateAleTotal();

        }

        private void ThreatDataCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (DataTable item in e.NewItems)
                {
                    item.PropertyChanged += ThreatData_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (DataTable item in e.OldItems)
                {
                    item.PropertyChanged -= ThreatData_PropertyChanged;
                }
            }

            UpdateAleTotal();
        }

        private void ThreatData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DataTable.ALE) ||
                e.PropertyName == nameof(DataTable.SLE) ||
                e.PropertyName == nameof(DataTable.EF) ||
                e.PropertyName == nameof(DataTable.ARO))
            {
                UpdateAleTotal();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == "Low" || textBox.Text == "Низкий")
            {
                textBox.Background = Brushes.Yellow;
                textBox.Foreground = Brushes.Black;
            }
            else if (textBox.Text == "Medium" || textBox.Text == "Средний")
            {
                textBox.Background = Brushes.Orange;
                textBox.Foreground = Brushes.Black;
            }
            else if(textBox.Text == "High" || textBox.Text == "Высокий")
            {
                textBox.Background = Brushes.Red;
                textBox.Foreground = Brushes.White;
            }
            else if (textBox.Text == "Critical" || textBox.Text == "Критический")
            {
                textBox.Background = Brushes.DarkRed;
                textBox.Foreground = Brushes.White;

            }
            else
            {
                textBox.Background = Brushes.Green;
                textBox.Foreground = Brushes.White;
            }
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAleTotal();
        }

        private void UpdateAleTotal()
        {
            double totalAle = 0;

            foreach (var data in ThreatDataCollection)
            {
                totalAle += data.ALE;
            }
            var prefix = DataContext is ViewModelLocator locator
                ? locator.Localization.RiskTotalPrefix
                : "Estimated total loss exposure: $";
            TextBoxResult.Text = prefix + totalAle.ToString(CultureInfo.InvariantCulture);
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
