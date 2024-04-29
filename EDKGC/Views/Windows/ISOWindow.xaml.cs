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

        }

        
    }
}