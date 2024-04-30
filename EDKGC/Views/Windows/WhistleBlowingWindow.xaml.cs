using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
