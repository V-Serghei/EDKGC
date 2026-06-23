using System.Windows;
using System.Windows.Controls;
using EDKGC.ViewModel;
using EDKGC.ViewModel.CentralSolutions;

namespace EDKGC.Views.Windows
{
    public partial class CentralWindow : Window
    {
        public CentralWindow()
        {
            InitializeComponent();
            DataContext = Application.Current.Resources["Locator"];
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is CentralViewModel vm)
                vm.SelectionChangedCommand.Execute(sender);
        }

        private void CopyCentralResult_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModelLocator locator && !string.IsNullOrEmpty(locator.Central.EncryptTextAl))
                Clipboard.SetText(locator.Central.EncryptTextAl);
        }

        private void CopySignature_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModelLocator locator && !string.IsNullOrEmpty(locator.Central.EncryptVerTextBoxS))
                Clipboard.SetText(locator.Central.EncryptVerTextBoxS);
        }

        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
