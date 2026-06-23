using System.Windows;
using System.Windows.Controls;
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
    }
}
