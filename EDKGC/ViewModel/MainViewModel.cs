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
using System.Configuration;
using System.Windows.Controls;

namespace EDKGC.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            #region CommandInf

            ShowButtonsCommand = new LCommand(ExecuteShowButtonsCommand);

            HideElementCommand = new LCommand(HideElement);
            #endregion

        }

        public LCommand HideElementCommand { get; private set; }
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

        private void HideElement(object parameter)
        {
            if (parameter is Button button)
            {
                button.Visibility = Visibility.Collapsed;
            }
            else if (parameter is TextBlock textBlock)
            {
                textBlock.Visibility = Visibility.Collapsed;
            }
            else if (parameter is Rectangle rect)
            {
                rect.Visibility = Visibility.Collapsed;
            }
        }


        private void ExecuteShowButtonsCommand(object parameter)
        {
            ButtonsPanelVisibility = Visibility.Visible;
            HideElement(parameter);
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


        private Brush _button1Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDB755A"));
        public Brush Button1Color
        {
            get { return _button1Color; }
            set
            {
                _button1Color = value;
                RaisePropertyChanged(nameof(Button1Color));
            }
        }
        private Brush _button2Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDB755A"));
        public Brush Button2Color
        {
            get { return _button2Color; }
            set
            {
                _button2Color = value;
                RaisePropertyChanged(nameof(Button2Color));
            }
        }
        private Brush _button3Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDB755A"));
        public Brush Button3Color
        {
            get { return _button3Color; }
            set
            {
                _button3Color = value;
                RaisePropertyChanged(nameof(Button3Color));
            }
        }
        private Brush _button4Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF332825"));
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