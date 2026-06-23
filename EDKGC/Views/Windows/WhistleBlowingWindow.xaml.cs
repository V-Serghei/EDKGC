using System.Windows;
using EDKGC.ViewModel;

namespace EDKGC.Views.Windows
{
    /// <summary>
    /// Interaction logic for WhistleBlowingWindow.xaml
    /// </summary>
    public partial class WhistleBlowingWindow : Window
    {
        public WhistleBlowingWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelLocator();

        }
    }
}
