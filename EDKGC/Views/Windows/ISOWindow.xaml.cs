using System.Windows;
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
        public ISOWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelLocator();
           
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