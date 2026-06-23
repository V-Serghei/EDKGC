using System;
using System.ComponentModel;
using System.Windows.Input;
using EDKGC.Infrastructure.Command;

namespace EDKGC.ViewModel.SatelliteWindows
{
    public class ConfirmationWindowViewModel : INotifyPropertyChanged
    {
        private string _title = "Confirmation";
        public string Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public ICommand YesCommand { get; }
        public ICommand NoCommand { get; }

        public event EventHandler<bool> ConfirmationResult;
        public event PropertyChangedEventHandler PropertyChanged;

        public ConfirmationWindowViewModel()
        {
            YesCommand = new LCommand(_ => OnConfirmationResult(true));
            NoCommand  = new LCommand(_ => OnConfirmationResult(false));
        }

        protected virtual void OnConfirmationResult(bool result) =>
            ConfirmationResult?.Invoke(this, result);

        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
