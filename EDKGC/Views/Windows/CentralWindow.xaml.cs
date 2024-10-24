﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using EDKGC.Infrastructure.Command.Control;
using EDKGC.ViewModel;
using EDKGC.ViewModel.CentralSolutions;

namespace EDKGC.Views.Windows
{
    /// <summary>
    /// Interaction logic for CentralWindow.xaml
    /// </summary>
    public partial class CentralWindow : Window
    {
       
        public CentralWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelLocator();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as CentralViewModel; 
            viewModel?.SelectionChangedCommand.Execute(sender); 
        }

       
    }
}
