using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EDKGC.ViewModel.CentralSolutions;

namespace EDKGC.Infrastructure.Command.Control
{
    public partial class CustomDropdownButton : UserControl
    {
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<string>), typeof(CustomDropdownButton));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(string), typeof(CustomDropdownButton));

        public ObservableCollection<string> Items
        {
            get => (ObservableCollection<string>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public string SelectedItem
        {
            get => (string)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public CustomDropdownButton()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DropdownPopup.IsOpen = !DropdownPopup.IsOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ListBox listBox)) return;
            if (listBox.SelectedItem == null) return;
            SelectedItem = listBox.SelectedItem.ToString();
            DropdownPopup.IsOpen = false;
        }
        
    }
}