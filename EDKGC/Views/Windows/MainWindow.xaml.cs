using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using EDKGC.Infrastructure.Command.BasicCommands;
using EDKGC.ViewModel;

namespace EDKGC.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelLocator();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CentralWindow window = new CentralWindow();

            window.Show();

            this.Hide();
        }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rectangle = sender as Rectangle;
            if (rectangle != null)
            {
                // Изменяем цвет прямоугольника по вашей логике
                rectangle.Fill = Brushes.Blue; // Например, изменяем на синий цвет
            }
        }
       



    }
}
