using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using EDKGC.Models;
using GalaSoft.MvvmLight;


namespace EDKGC.ViewModel.CentralSolutions
{
    public class CentralViewModel : ViewModelBase, IDisposable
    {
        public CentralViewModel()
        {
            AesSymmetricEncryptionM = new AesSymmetricEncryption();
           
            CloseAppCommand = new RelayCommand(OnCloseAppCommandExecuted, CanCloseAppCommandExecuted);

            GenKeyAes = new RelayCommand(AesGenKey);

            EncryptText = new RelayCommand(AesEncryptTextB);

            FirstItems = new ObservableCollection<string>
            {
                "AES",
                "DES",
                "3DES",
                "Kuznyechik",
                "Blowfish",
                "Twofish",
                "Serpent"


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


        #region AES

        /// <summary>
        /// AES symmetric encryption
        /// </summary>
        public ICommand GenKeyAes { get; set; }
        public ICommand EncryptText { get; set; }
        public AesSymmetricEncryption AesSymmetricEncryptionM { get; set; }
        public string Base64String { get; set; }
        private string _aesKeyText;
        private string _aesEncryptText;
        private string _aesTextNonEncrypt;


        public string AesTextNonEncrypt
        {
            get => _aesTextNonEncrypt;
            set => Set(ref _aesTextNonEncrypt, value);
        }
        public string AesEncryptText
        {
            get => _aesEncryptText;
            set => Set(ref _aesEncryptText, value);
        }

        public string AesKeyText
        {
            get => _aesKeyText;
            set => Set(ref _aesKeyText, value);
        }
        public void AesGenKey()
        {
            var key = AesSymmetricEncryptionM.GenKeyAesAlg();
            Base64String = Convert.ToBase64String(AesSymmetricEncryptionM.Key);
            //Base64String = BitConverter.ToString(AesSymmetricEncryptionM.Key).Replace("-", " ");
            AesKeyText = Base64String;

        }

        public void AesEncryptTextB()
        {

           var encryptText =  AesSymmetricEncryptionM.Encrypting(_aesTextNonEncrypt);
           Base64String = Convert.ToBase64String(encryptText);
           AesEncryptText = Base64String;
           AesTextNonEncrypt = AesSymmetricEncryptionM.EnterText;

        }





        #endregion


        public void Dispose()
        {
           
        }
    }
}
