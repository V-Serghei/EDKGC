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
using EDKGC.Models.SymmetricEncryption;
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
        readonly Encoding _encoding = Encoding.Default;


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

            #region encryption algorithms

            AesSymmetricEncryptionM = new AesSymmetricEncryption();

            DesSymmetricEncryptionM = new DESsymmetricEncryption();

            GenKeyAes = new RelayCommand(GenKeyAl);

            EncryptText = new RelayCommand(EncryptTextB);

            SwapEncryptDecrypt = new RelayCommand(SwapEnDecrypt);

            Items = new ObservableCollection<string>() { "Aes", "DES", "3DES", "SEAL", "Blowfish", "Twofish", "Serpent" };
            SelectionChangedCommand = new RelayCommand(UpdateCommand);

            #endregion

            CloseAppCommand = new RelayCommand(OnCloseAppCommandExecuted, CanCloseAppCommandExecuted);

            
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

        private string _selectedItemText = "Aes";
        public string SelectedItemText
        {
            get => _selectedItemText;
            set
            {
                Set(ref _selectedItemText, value);
                UpdateCommand();
            }
        }

        private void UpdateCommand()
        {
            KeyTextAl = "Generate Key!";
            TextNonEncrypt = EncryptTextAl;
            if (ButtonEffect == Effect.Decrypt)
            {
                SwapEnDecrypt();
            }

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




        #region Symmetric encryption
        /// <summary>
        /// Symmetric encryption
        /// </summary>

        private SymmetricEncryption _algorithmSymmetricEncryption = SymmetricEncryption.Aes;

        

        /// <summary>
        /// AES symmetric encryption
        /// </summary>

        public ICommand GenKeyAes { get; set; }
        public ICommand EncryptText { get; set; }
        public ICommand SwapEncryptDecrypt { get; set; }
        public AesSymmetricEncryption AesSymmetricEncryptionM { get; set; }
        public DESsymmetricEncryption DesSymmetricEncryptionM { get; set; }
        public string ConvertByteStringContainer { get; set; }
        private string _keyTextAl;
        private string _encryptTextAl;
        private string _textNonEncrypt = "Enter text";
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

        public string TextNonEncrypt
        {
            get => _textNonEncrypt;
            set => Set(ref _textNonEncrypt, value);
        }
        public string EncryptTextAl
        {
            get => _encryptTextAl;
            set => Set(ref _encryptTextAl, value);
        }

        public string KeyTextAl
        {
            get => _keyTextAl;
            set => Set(ref _keyTextAl, value);
        }
        public void GenKeyAl()
        {

            switch (_algorithmSymmetricEncryption)
            {
                case SymmetricEncryption.Aes:
                {
                    var key = AesSymmetricEncryptionM.GenKeyAesAlg();
                    ConvertByteStringContainer = _encoding.GetString(AesSymmetricEncryptionM.Key);
                    //ConvertByteStringContainer = BitConverter.ToString(AesSymmetricEncryptionM.Key).Replace("-", " ");
                    KeyTextAl = ConvertByteStringContainer;
                    break;
                }
                case SymmetricEncryption.DES:
                {
                    var key = DesSymmetricEncryptionM.GenKeyDes();
                    KeyTextAl = _encoding.GetString(key);
                    break;
                }
                case SymmetricEncryption.TripleDES:
                {

                    break;
                }
                default:
                {
                    break;
                }
            }

        }

        public void EncryptTextB()
        {

            switch (_algorithmSymmetricEncryption)
            {
                case SymmetricEncryption.Aes:
                {
                    

                    switch (ButtonEffect)
                    {
                        case Effect.Encrypt:
                            if (TextNonEncrypt != null)
                            {
                                ConvertByteStringContainer =
                                    _encoding.GetString(AesSymmetricEncryptionM.Encrypting(TextNonEncrypt));
                                EncryptTextAl = ConvertByteStringContainer;
                                TextNonEncrypt = AesSymmetricEncryptionM.EnterText;
                            }
                            else TextNonEncrypt = "Enter text";
                            
                            break;
                        case Effect.Decrypt:
                            var res = AesSymmetricEncryptionM.Decrypt(TextNonEncrypt);
                            if (res == null) EncryptTextAl = "Inappropriate key";
                            else
                            {
                                ConvertByteStringContainer = _encoding.GetString(res);
                                EncryptTextAl = ConvertByteStringContainer;
                            }
                            
                            break;
                    }

                    break;
                }
                case SymmetricEncryption.DES:
                {
                    switch (ButtonEffect)
                    {
                        case Effect.Encrypt:
                            ConvertByteStringContainer = _encoding.GetString(DesSymmetricEncryptionM.GetEncryptTextEdc(TextNonEncrypt));
                            EncryptTextAl = ConvertByteStringContainer;
                            TextNonEncrypt = DesSymmetricEncryptionM.EnterText;
                            break;
                        case Effect.Decrypt:
                            ConvertByteStringContainer = DesSymmetricEncryptionM.GetDecryptTextEbc(TextNonEncrypt) ?? "Inappropriate key";
                            EncryptTextAl = ConvertByteStringContainer;
                            break;
                        case Effect.None:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                }
                case SymmetricEncryption.TripleDES:
                {
                    break;
                }
                default:
                {
                    break;
                }
            }

            


        }

        public void SwapEnDecrypt()
        {
            switch (ButtonEffect)
            {
                case Effect.Encrypt:
                    ButtonEffect = Effect.Decrypt;
                    ButtonEDecrypt = "Decrypt";
                    TextNonEncrypt = EncryptTextAl;
                    EncryptTextAl = "";
                    break;
                case Effect.Decrypt:
                    ButtonEffect = Effect.Encrypt;
                    ButtonEDecrypt = "Encrypt";
                    if (EncryptTextAl == "") TextNonEncrypt = "";
                    else
                    {
                        TextNonEncrypt = EncryptTextAl;
                    }
                    EncryptTextAl = "";
                    break;
                default:
                    ButtonEffect = Effect.Encrypt;
                    break;
            }

        }





        


        #region Des
        ////
        ////symmetric encryption algorithm Des
        ////




        #endregion


        #endregion

        public void Dispose()
        {
           
        }
    }
}
