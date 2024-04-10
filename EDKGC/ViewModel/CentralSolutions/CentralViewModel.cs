﻿using System;
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
using EDKGC.Models.AsymmetricAlgorithms;
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

        private RsaAsymmetricalAlModel _rsaAsymmetricalAlModel;

        public string ConvertByteStringContainer { get; set; }

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
            EncryptAsymmetricalCommand = new RelayCommand(EncryptDecryptAs);
            GenKeyAsymmetrical1Command = new RelayCommand(GenKeyAs1);
            GenKeyAsymmetrical2Command = new RelayCommand(GenKeyAs2);
            _rsaAsymmetricalAlModel = new RsaAsymmetricalAlModel();
            _itemsAsymAl = new ObservableCollection<string>
            {
                "RSA",
                "DiffieHellman",
                "ElGamal",
                "ECC"
            };

            #region encryption algorithms

            AesSymmetricEncryptionM = new AesSymmetricEncryption();

            DesSymmetricEncryptionM = new DESsymmetricEncryption();

            GenKeySymmetricalCommand = new RelayCommand(GenKeyAl);

            EncryptTextCommand = new RelayCommand(EncryptDecryptTextSym);

            SwapEncryptDecryptcCommand = new RelayCommand(SwapEnDecrypt);

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

        private Effect _buttonEffect = Effect.Encrypt;

        private AsymmetricAlgorithms _asymmetricAlgorithms = AsymmetricAlgorithms.RSA;

        private TypeOfAlgorithm _typeOfAlgorithm = TypeOfAlgorithm.Symmetrical;

        private string _encryptTextAl;

        private string _textNonEncrypt = "Enter text";

        private string _buttonEDecrypt = "Encrypt";


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

        public TypeOfAlgorithm TypeOfAlgorithm
        {
            get => _typeOfAlgorithm;
            set => Set(ref _typeOfAlgorithm, value);
        }

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
                TypeOfAlgorithm = TypeOfAlgorithm.Symmetrical;
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

            switch (TypeOfAlgorithm)
            {
                case TypeOfAlgorithm.Symmetrical:
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
                    else if (SelectedItemText == Items[1])
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
                    else if (SelectedItemText == Items[4])
                    {
                        _algorithmSymmetricEncryption = SymmetricEncryption.Blowfish;
                    }
                    else if (SelectedItemText == Items[5])
                    {
                        _algorithmSymmetricEncryption = SymmetricEncryption.Twofish;
                    }
                    else
                    {
                        _algorithmSymmetricEncryption = SymmetricEncryption.Serpent;
                    }

                    break;
                }
                case TypeOfAlgorithm.Asymmetrical:
                {
                    KeyTextAl1 = "Generate Key!";
                    KeyTextAl2 = KeyTextAl1;
                    TextNonEncrypt = EncryptTextAl;
                    if (ButtonEffect == Effect.Decrypt)
                    {
                        SwapEnDecrypt();
                    }

                    if (SelectedItemText == ItemsAsymAl[0])
                    {
                        _asymmetricAlgorithms = AsymmetricAlgorithms.RSA;
                    }else if (SelectedItemText == ItemsAsymAl[1])
                    {
                        _asymmetricAlgorithms = AsymmetricAlgorithms.DiffieHellman;
                    }
                    else if (SelectedItemText == ItemsAsymAl[2])
                    {
                        _asymmetricAlgorithms = AsymmetricAlgorithms.ElGamal;
                    }
                    else if (SelectedItemText == ItemsAsymAl[3])
                    {
                        _asymmetricAlgorithms = AsymmetricAlgorithms.ECC;
                    }

                    break;
                }
            }
        }



        private ObservableCollection<string> _itemsAsymAl;
        public ObservableCollection<string> ItemsAsymAl
        {
            get => _itemsAsymAl;
            set
            {
                _itemsAsymAl = value;
                OnPropertyChanged(nameof(ItemsAsymAl));
            }
        }
        private string _selectedItemAsymAl;
        public string SelectedItemAsumAl
        {
            get => _selectedItemAsymAl;
            set
            {
                _selectedItemAsymAl = value;
                OnPropertyChanged(nameof(SelectedItemAsumAl));
                SelectedItemText = value;
                TypeOfAlgorithm = TypeOfAlgorithm.Asymmetrical;
            }
        }




        public Effect ButtonEffect
        {
            get => _buttonEffect;
            set => Set(ref _buttonEffect, value);
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

        public ICommand GenKeySymmetricalCommand { get; set; }
       
        public ICommand EncryptTextCommand { get; set; }

        public ICommand SwapEncryptDecryptcCommand { get; set; }

        public AesSymmetricEncryption AesSymmetricEncryptionM { get; set; }

        public DESsymmetricEncryption DesSymmetricEncryptionM { get; set; }

        private string _keyTextAl = "Generate Key!";

       

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
                    AesSymmetricEncryptionM.GenKeyAesAlg();
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

        public void EncryptDecryptTextSym()
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

        




    




        #endregion

        #region ASYMMETRIC ALGORITHMS
        /// <summary>
        /// ASYMMETRIC ALGORITHMS
        /// </summary>
        public ICommand GenKeyAsymmetrical1Command { get; set; }

        public ICommand GenKeyAsymmetrical2Command { get; set; }

        public ICommand EncryptAsymmetricalCommand { get; set; }



        private string _keyTextAl1 = "Generate public Key!";

        private string _keyTextAl2 = "Generate private Key!";

        public string KeyTextAl1
        {
            get => _keyTextAl1;
            set => Set(ref _keyTextAl1, value);
        }
        public string KeyTextAl2
        {
            get => _keyTextAl2;
            set => Set(ref _keyTextAl2, value);
        }

        private void GenKeyAs2()
        {
            KeyTextAl2 = _encoding.GetString(_rsaAsymmetricalAlModel.GenPrivateKey());
        }

        private void GenKeyAs1()
        {
            KeyTextAl1 = _encoding.GetString(_rsaAsymmetricalAlModel.GenPublicKey());

        }


        private void EncryptDecryptAs()
        {
            switch (_asymmetricAlgorithms)
            {
                
                case AsymmetricAlgorithms.RSA:
                {
                    switch (ButtonEffect)
                    {
                        case Effect.Encrypt:
                            if (TextNonEncrypt != null)
                            {
                                ConvertByteStringContainer =
                                    _encoding.GetString(_rsaAsymmetricalAlModel.EncryptTextRsa(TextNonEncrypt));
                                EncryptTextAl = ConvertByteStringContainer;
                                
                            }
                            else TextNonEncrypt = "Enter text";

                            break;
                        case Effect.Decrypt:
                            var res = _rsaAsymmetricalAlModel.DecryptTextRsa(_encoding.GetBytes(TextNonEncrypt));
                            if (res == null) EncryptTextAl = "Inappropriate key";
                            else
                            {
                                ConvertByteStringContainer = res;
                                EncryptTextAl = ConvertByteStringContainer;
                            }

                            break;
                    }


                    }
                    break;
                case AsymmetricAlgorithms.DiffieHellman:
                {

                }
                    break;
                case AsymmetricAlgorithms.ElGamal:
                {

                }
                    break;
                case AsymmetricAlgorithms.ECC:
                {

                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        public void Dispose()
        {
           
        }
    }
}
