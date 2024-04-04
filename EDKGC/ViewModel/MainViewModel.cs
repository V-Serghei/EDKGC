using System;
using System.Globalization;
using EDKGC.Views.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using EDKGC.Infrastructure.Command;

namespace EDKGC.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            #region CommandInf

            ShowButtonsCommand = new RelayCommand(ExecuteShowButtonsCommand);


            #endregion

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

        #region CommandInf

        /// <summary>
        /// CommandInf 
        /// </summary>

        


        


        #endregion


        private Brush _button1Color = Brushes.Red;
        public Brush Button1Color
        {
            get { return _button1Color; }
            set
            {
                _button1Color = value;
                RaisePropertyChanged(nameof(Button1Color));
            }
        }
        private Brush _button2Color = Brushes.BlueViolet;
        public Brush Button2Color
        {
            get { return _button2Color; }
            set
            {
                _button2Color = value;
                RaisePropertyChanged(nameof(Button2Color));
            }
        }
        private Brush _button3Color = Brushes.DarkOrange;
        public Brush Button3Color
        {
            get { return _button3Color; }
            set
            {
                _button3Color = value;
                RaisePropertyChanged(nameof(Button3Color));
            }
        }
        private Brush _button4Color = Brushes.Tan;
        public Brush Button4Color
        {
            get { return _button4Color; }
            set
            {
                _button4Color = value;
                RaisePropertyChanged(nameof(Button4Color));
            }
        }





    }
}