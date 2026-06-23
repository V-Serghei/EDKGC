using System;
using System.Windows;
using EDKGC.ViewModel.SatelliteWindows;

namespace EDKGC.Views.Windows
{
    public partial class ConfirmationWindow : Window
    {
        private ConfirmationWindowViewModel _viewModel;

        public ConfirmationWindow()
        {
            InitializeComponent();
            _viewModel = DataContext as ConfirmationWindowViewModel;
            if (_viewModel != null)
                _viewModel.ConfirmationResult += ConfirmationResult;
            Closed += ConfirmationWindow_Closed;
        }

        private void ConfirmationResult(object sender, bool result)
        {
            if (result)
                Application.Current.Shutdown();
            else
                Close();
        }

        private void ConfirmationWindow_Closed(object sender, EventArgs e)
        {
            if (_viewModel != null)
                _viewModel.ConfirmationResult -= ConfirmationResult;
        }
    }
}
