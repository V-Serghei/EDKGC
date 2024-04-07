using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EDKGC.Views.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace EDKGC.ViewModel.CentralSolutions
{
    public class CentralViewModel : ViewModelBase, IDisposable
    {
        public CentralViewModel()
        {
            CloseAppCommand = new RelayCommand(OnCloseAppCommandExecuted, CanCloseAppCommandExecuted);
            FirstItems = new ObservableCollection<string>
            {
                "Item 1",
                "Item 2",
                "Item 3"
            };
            SecondItems = new ObservableCollection<string>
            {
                "Item 4",
                "Item 5",
                "Item 3"
            };
            ThirdItems = new ObservableCollection<string>
            {
                "Item 9",
                "Item 2",
                "Item 3"
            };
        }

        #region Custom Dropdown Button

        /// <summary>
        /// Custom Dropdown Button
        /// Personal Popup Box Selection
        /// </summary>

        public string SelectedFirstItem { get; set; }
        public string SelectedSecondItem { get; set; }
        public string SelectedThirdItem { get; set; }

        public ObservableCollection<string> FirstItems { get; set; }
        public ObservableCollection<string> SecondItems { get; set; }
        public ObservableCollection<string> ThirdItems { get; set; }
        

        #endregion

        #region CloseAppCommand

        public ICommand CloseAppCommand { get; }

        private static bool CanCloseAppCommandExecuted() => true;

        public void OnCloseAppCommandExecuted()
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region Window title
        /// <summary>
        /// Window title
        /// </summary>

        private string _title = "EDKGC Central";

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


        #region StatusB

        /// <summary>
        /// Message status bar
        /// </summary>

        private string _status = "Processing";

        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }

        #endregion

        

    public void Dispose()
        {
           
        }
    }
}
