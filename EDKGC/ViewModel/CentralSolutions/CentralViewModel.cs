using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using EDKGC.Enams;
using EDKGC.Models;
using GalaSoft.MvvmLight;
using Org.BouncyCastle.Utilities;


namespace EDKGC.ViewModel.CentralSolutions
{
    public class CentralViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region basic tools

        /// <summary>
        /// PropertyChanged
        /// And
        /// Encoding
        /// </summary>
        readonly Encoding _encoding = Encoding.UTF7;

        private SymmetricEncryption _algorithmSymmetricEncryption = SymmetricEncryption.Aes;

        public new event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected new bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }


        #endregion

        public CentralViewModel()
        {
            AesSymmetricEncryptionM = new AesSymmetricEncryption();
           
            CloseAppCommand = new RelayCommand(OnCloseAppCommandExecuted, CanCloseAppCommandExecuted);

            GenKeyAes = new RelayCommand(AesGenKey);

            EncryptText = new RelayCommand(AesEncryptTextB);

            SwapEncryptDecrypt = new RelayCommand(SwapEnDecrypt);


            Items = new ObservableCollection<string>() { "Aes", "DES", "3DES", "SEAL", "Blowfish" , "Twofish" , "Serpent" };
            SelectionChangedCommand = new RelayCommand(UpdateCommand);
        }

        #region Algorithm selection list

        /// <summary>
        /// Algorithm selection list
        /// </summary>
        public ICommand SelectionChangedCommand { get; set; }

        private ObservableCollection<string> _items;
        public ObservableCollection<string> Items
        {
            get => _items;
            set
            {
                Set(ref _items, value);
                SelectedItemText = SelectedItem;
            } 
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                SelectedItemText = value;
            }
        }

        private string _selectedItemText;
        public string SelectedItemText
        {
            get => _selectedItemText;
            set => Set(ref _selectedItemText, value);
        }

        private void UpdateCommand()
        {
            if (SelectedItemText == Items[0])
            {
                _algorithmSymmetricEncryption = SymmetricEncryption.Aes;
            }
            else if(SelectedItemText == Items[1])
            {
                _algorithmSymmetricEncryption = SymmetricEncryption.DES;
            }
            else if (SelectedItemText == Items[2])
            {
                _algorithmSymmetricEncryption = SymmetricEncryption.TripleDES;
            }
            else if (SelectedItemText == Items[3])
            {
                _algorithmSymmetricEncryption = SymmetricEncryption.SEAL;
            }
            else if (_selectedItemText == Items[4])
            {
                _algorithmSymmetricEncryption = SymmetricEncryption.Blowfish;
            }
            else if (_selectedItemText == Items[5])
            {
                _algorithmSymmetricEncryption = SymmetricEncryption.Twofish;
            }
            else
            {
                _algorithmSymmetricEncryption = SymmetricEncryption.Serpent;
            }
        }


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
        public ICommand SwapEncryptDecrypt { get; set; }
        public AesSymmetricEncryption AesSymmetricEncryptionM { get; set; }
        public string Base64String { get; set; }
        private string _aesKeyText;
        private string _aesEncryptText;
        private string _aesTextNonEncrypt = "Enter text";
        private string _buttonEDecrypt = "Encrypt";
        private Effect _buttonEffect = Effect.Encrypt;

       


        public Effect ButtonEffect
        {
            get => _buttonEffect;
            set => Set(ref _buttonEffect, value);
        }

        public string ButtonEDecrypt
        {
            get => _buttonEDecrypt;
            set => Set(ref _buttonEDecrypt, value);
        }

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
            Base64String = _encoding.GetString(AesSymmetricEncryptionM.Key);
            //Base64String = BitConverter.ToString(AesSymmetricEncryptionM.Key).Replace("-", " ");
            AesKeyText = Base64String;

        }

        public void AesEncryptTextB()
        {


            if (ButtonEffect == Effect.Encrypt)
            {
                Base64String = _encoding.GetString(AesSymmetricEncryptionM.Encrypting(AesTextNonEncrypt));
                AesEncryptText = Base64String;
                AesTextNonEncrypt = AesSymmetricEncryptionM.EnterText;

            }
            else if (ButtonEffect == Effect.Decrypt)
            {
                Base64String = _encoding.GetString(AesSymmetricEncryptionM.Decrypt(AesTextNonEncrypt));
                AesEncryptText = Base64String;
            }
           

        }

        public void SwapEnDecrypt()
        {
            switch (ButtonEffect)
            {
                case Effect.Encrypt:
                    ButtonEffect = Effect.Decrypt;
                    ButtonEDecrypt = "Decrypt";
                    AesTextNonEncrypt = AesEncryptText;
                    AesEncryptText = "";
                    break;
                case Effect.Decrypt:
                    ButtonEffect = Effect.Encrypt;
                    ButtonEDecrypt = "Encrypt";
                    if (AesEncryptText == "") AesTextNonEncrypt = "";
                    else
                    {
                        AesTextNonEncrypt = AesEncryptText;
                    }
                    AesEncryptText = "";
                    break;
                default:
                    ButtonEffect = Effect.Encrypt;
                    break;
            }
            
        }







        #endregion


        public void Dispose()
        {
           
        }
    }
}
