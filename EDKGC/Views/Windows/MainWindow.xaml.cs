using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

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

        private void Button_Click_Iso(object sender, MouseButtonEventArgs e)
        {
            new ISOWindow().Show();
            new WhistleBlowingWindow().Show();
            Close();
        }
    }
}
