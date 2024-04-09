using EDKGC.Views.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using EDKGC.Infrastructure.Command;
using System.Windows.Controls;

namespace EDKGC.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public RelayCommand ExitCommand { get; private set; }
        public LCommand HideElementCommand { get; private set; }
        public ICommand ShowButtonsCommand { get; private set; }
        private Visibility _buttonsPanelVisibility = Visibility.Collapsed;
        public Visibility ButtonsPanelVisibility
        {
            get => _buttonsPanelVisibility;
            set
            {
                if (_buttonsPanelVisibility == value) return;
                _buttonsPanelVisibility = value;
                RaisePropertyChanged(nameof(ButtonsPanelVisibility));
            }
        }

       
       
        public MainViewModel()
        {

            #region CommandInf

            ShowButtonsCommand = new LCommand(ExecuteShowButtonsCommand);

            HideElementCommand = new LCommand(HideElement);

            ExitCommand = new RelayCommand(ExecutExitCommand);

            #endregion


        }

        private void ExecutExitCommand()
        {
            var confW = new ConfirmationWindow();
            confW.Show();

        }


        #region Hide Element - Execute Show Buttons Command
        /// <summary>
        /// Commands to drill and hide elements
        /// </summary>
        /// <param name="parameter"></param>



        private static void HideElement(object parameter)
        {
            switch (parameter)
            {
                case Button button:
                    button.Visibility = Visibility.Collapsed;
                    break;
                case TextBlock textBlock:
                    textBlock.Visibility = Visibility.Collapsed;
                    break;
                case Rectangle rect:
                    rect.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void ExecuteShowButtonsCommand(object parameter)
        {
            ButtonsPanelVisibility = Visibility.Visible;
            HideElement(parameter);
        }

        #endregion


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