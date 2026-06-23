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

        private void Button_Click_Iso(object sender, RoutedEventArgs e)
        {
            new ISOWindow().Show();
            new WhistleBlowingWindow().Show();
            Close();
        }

        private void BasicInformation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "EDKGC modules:\n\n" +
                "Symmetric encryption: AES, DES, 3DES, Blowfish, Twofish, Serpent.\n" +
                "Asymmetric encryption: RSA demo with public/private key switching.\n" +
                "Signing: hash generation, RSA signature, decrypt and verify flow.\n" +
                "ISO 27001: questionnaire and risk analysis tools.\n\n" +
                "Note: RSA can encrypt only short messages directly. For long text, encrypt a hash or a symmetric key.",
                "Basic information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
