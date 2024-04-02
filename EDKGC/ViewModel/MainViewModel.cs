using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Input;

namespace EDKGC.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            ShowButtonsCommand = new RelayCommand(ExecuteShowButtonsCommand);

        }

        public ICommand ShowButtonsCommand { get; private set; }
        private Visibility _buttonsPanelVisibility = Visibility.Collapsed;
        public Visibility ButtonsPanelVisibility
        {
            get { return _buttonsPanelVisibility; }
            set
            {
                if (_buttonsPanelVisibility != value)
                {
                    _buttonsPanelVisibility = value;
                    RaisePropertyChanged(nameof(ButtonsPanelVisibility));
                }
            }
        }

        private void ExecuteShowButtonsCommand()
        {
            ButtonsPanelVisibility = Visibility.Visible;
        }

        #region Window title
        /// <summary>
        /// Window title
        /// </summary>

        private string _title = "Welcome to EDKGC";

        public string Title
        {
            get => _title;
            set =>
                //if(Equals(_title,value))return;
                //_title = value;
                //RaisePropertyChanged(nameof(Title));
                Set(ref _title, value);

        }

        #endregion




        
    }
}