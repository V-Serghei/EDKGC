using System.Windows;
using EDKGC.ViewModel;

namespace EDKGC.Views.Windows
{
    /// <summary>
    /// Interaction logic for IsoResultsWidow.xaml
    /// </summary>
    public partial class IsoResultsWidow : Window
    {
        public IsoResultsWidow()
        {
            InitializeComponent();
            DataContext = new ViewModelLocator();

        }

        
    }
}
