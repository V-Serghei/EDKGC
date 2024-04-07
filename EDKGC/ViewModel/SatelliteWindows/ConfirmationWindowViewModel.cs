using System;
using System.Windows.Input;

namespace EDKGC.ViewModel.SatelliteWindows
{
    public class ConfirmationWindowViewModel : ViewModelBase
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

        public ConfirmationWindowViewModel()
        {
            Title = "Confirmation";

            YesCommand = new RelayCommand(ExecuteYesCommand);
            NoCommand = new RelayCommand(ExecuteNoCommand);
        }

        private void ExecuteYesCommand(object parameter)
        {
            OnConfirmationResult(true);
        }

        private void ExecuteNoCommand(object parameter)
        {
            OnConfirmationResult(false);
        }

        protected virtual void OnConfirmationResult(bool result)
        {
            ConfirmationResult?.Invoke(this, result);
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }

    public abstract class ViewModelBase : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
