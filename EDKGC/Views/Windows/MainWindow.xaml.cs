using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using EDKGC.ViewModel;

namespace EDKGC.Views.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Application.Current.Resources["Locator"];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new CentralWindow().Show();
            Hide();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle rectangle)
                rectangle.Fill = Brushes.Blue;
        }

        private void Button_Click_Iso(object sender, RoutedEventArgs e)
        {
            new ISOWindow().Show();
            new WhistleBlowingWindow().Show();
            Close();
        }

        private void BasicInformation_Click(object sender, RoutedEventArgs e)
        {
            var locator = Application.Current.Resources["Locator"] as ViewModelLocator;
            MessageBox.Show(
                locator?.Localization.BasicInfoMessage ?? "EDKGC modules",
                locator?.Localization.BasicInformation ?? "Basic information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
