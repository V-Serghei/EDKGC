using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Input;
using EDKGC.Enams;
using EDKGC.Encryption.GeneralTools;
using EDKGC.Models;
using EDKGC.Models.AsymmetricAlgorithms;
using EDKGC.Models.ElectronicSignature;
using EDKGC.Models.SymmetricEncryption;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

//TODO:Review all input variations and protect against critical errors.
//Testing is required

//TODO-CHECK:Add exception processing

//correctly reorganize code

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
        readonly Encoding _encoding = Encoding.UTF8;

        private RsaAsymmetricalAlModel _rsaAsymmetricalAlModel;
        private DiffieHellmanAsymmetricalAlModel _diffieHellmanAsymmetricalAlModel;
        private ElGamalAsymmetricalAlModel _elGamalAsymmetricalAlModel;
        private EccAsymmetricalAlModel _eccAsymmetricalAlModel;
        private LocalizationViewModel _localization;

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

        public CentralViewModel( )
        {
            VerifySignatureCommand = new RelayCommand(VerifySignRsa);
            DecryptEncryptHashCommand = new RelayCommand(DecryptTextHashS);
            GenHashAndEncryptSignatureCommand = new RelayCommand(GenSignature);
            ButGenKeyPCommand = new RelayCommand(GenKeyPeir);
            _electronicSignatureRsa = new ElectronicSignatureRSA();
            EncryptAsymmetricalCommand = new RelayCommand(EncryptDecryptAs);

            GenKeyAsymmetrical2Command = new RelayCommand(GenKeyAs);
            _rsaAsymmetricalAlModel = new RsaAsymmetricalAlModel();
            _diffieHellmanAsymmetricalAlModel = new DiffieHellmanAsymmetricalAlModel();
            _elGamalAsymmetricalAlModel = new ElGamalAsymmetricalAlModel();
            _eccAsymmetricalAlModel = new EccAsymmetricalAlModel();
            _electronicSignatureRsa.SetKeyPair(_rsaAsymmetricalAlModel.GetKeyP());
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

            TripleDesSymmetricEncryptionM = new TripleDesSymmetricEncryption();

            BlowfishSymmetricEncryptionM = new BlowfishSymmetricEncryption();

            TwofishSymmetricEncryptionM = new TwofishSymmetricEncryption();

            SerpentSymmetricEncryptionM = new SerpentSymmetricEncryption();

            GenKeySymmetricalCommand = new RelayCommand(GenKeyAl);

            EncryptTextCommand = new RelayCommand(EncryptDecryptTextSym);

            SwapEncryptDecryptcCommand = new RelayCommand(SwapEnDecrypt);

            SwapKeyCommand = new RelayCommand(SwapKeyR);

            Items = new ObservableCollection<string>() { "Aes", "DES", "3DES", "Blowfish", "Twofish", "Serpent" };
            SelectionChangedCommand = new RelayCommand(UpdateCommand);

            #endregion

            CloseAppCommand = new RelayCommand(OnCloseAppCommandExecuted, CanCloseAppCommandExecuted);
            AboutProgramCommand = new RelayCommand(ShowAboutProgram);

            SetupLocalization();
        }

        private void SetupLocalization()
        {
            try
            {
                _localization = SimpleIoc.Default.GetInstance<LocalizationViewModel>();
                _localization.PropertyChanged += (_, _) => RefreshLocalizedText();
                RefreshLocalizedText();
            }
            catch
            {
                _localization = null;
            }
        }

        private string L(string key) => _localization?.Text(key) ?? key;

        private void RefreshLocalizedText()
        {
            ButtonEDecrypt = ButtonEffect == Effect.Decrypt ? L("Decrypt") : L("Encrypt");
            SignatureDecryptButtonText = _isSignatureDecrypted ? L("Encrypt") : L("Decrypt");

            if (TextNonEncrypt == "Enter text" || TextNonEncrypt == "Введите текст")
            {
                TextNonEncrypt = L("EnterText");
            }

            if (EnterTextS == "Enter text" || EnterTextS == "Введите текст")
            {
                EnterTextS = L("EnterText");
            }

            if (HashEntTextS == "Expect hash" || HashEntTextS == "Ожидается хеш")
            {
                HashEntTextS = L("ExpectHash");
            }

            if (EncryptVerTextBoxS == "Expect Encrypt" || EncryptVerTextBoxS == "Ожидается подпись")
            {
                EncryptVerTextBoxS = L("ExpectEncrypt");
            }

            if (KeyEncState == "Public Key!" || KeyEncState == "Публичный ключ!")
            {
                KeyEncState = L("PublicKey") + "!";
            }
            else if (KeyEncState == "Private Key!" || KeyEncState == "Приватный ключ!")
            {
                KeyEncState = L("PrivateKey") + "!";
            }

            if (KeyTextAl1 == "Generate Key!" || KeyTextAl1 == "Сгенерируйте ключ!")
            {
                KeyTextAl1 = L("GenerateKeyPrompt") + "!";
            }

            if (KeyTextAl2 == "Generate Key!" || KeyTextAl2 == "Сгенерируйте ключ!")
            {
                KeyTextAl2 = L("GenerateKeyPrompt") + "!";
            }

            if (KeyTextAl == "Generate Key!" || KeyTextAl == "Сгенерируйте ключ!")
            {
                KeyTextAl = L("GenerateKeyPrompt") + "!";
            }

            if (EncryptTextAl == "Inappropriate key" || EncryptTextAl == "Неверный ключ")
            {
                EncryptTextAl = L("InappropriateKey");
            }
            else if (EncryptTextAl == "Inappropriate key or text" || EncryptTextAl == "Неверный ключ или текст")
            {
                EncryptTextAl = L("InappropriateKeyOrText");
            }
            else if (EncryptTextAl == "SEAL is not supported" || EncryptTextAl == "SEAL не поддерживается")
            {
                EncryptTextAl = L("SealNotSupported");
            }

            if (EncryptVerTextBoxS == "Inappropriate signature" || EncryptVerTextBoxS == "Неверная подпись")
            {
                EncryptVerTextBoxS = L("InappropriateSignature");
            }
            else if (EncryptVerTextBoxS == "Generate signature first" || EncryptVerTextBoxS == "Сначала сгенерируйте подпись")
            {
                EncryptVerTextBoxS = L("GenerateSignatureFirst");
            }
            else if (EncryptVerTextBoxS == "Could not generate signature" || EncryptVerTextBoxS == "Не удалось сгенерировать подпись")
            {
                EncryptVerTextBoxS = L("CouldNotGenerateSignature");
            }

            if (TextResp == "Pass" || TextResp == "Пройдено")
            {
                TextResp = L("Pass");
            }
            else if (TextResp == "Hazard!!!" || TextResp == "Ошибка!!!")
            {
                TextResp = L("Hazard");
            }
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
                    KeyTextAl = L("GenerateKeyPrompt") + "!";
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
                        _algorithmSymmetricEncryption = SymmetricEncryption.Blowfish;
                    }
                    else if (SelectedItemText == Items[4])
                    {
                        _algorithmSymmetricEncryption = SymmetricEncryption.Twofish;
                    }
                    else if (SelectedItemText == Items[5])
                    {
                        _algorithmSymmetricEncryption = SymmetricEncryption.Serpent;
                    }

                    break;
                }
                case TypeOfAlgorithm.Asymmetrical:
                {
                    KeyTextAl1 = L("GenerateKeyPrompt") + "!";
                    KeyTextAl2 = KeyTextAl1;
                    EncryptTextAl = "";
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
        private string _selectedItemAsymAl = "RSA";
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

        private EKeyEff _eKeyEff = EKeyEff.Public;

        public EKeyEff KeyState
        {
            get => _eKeyEff;
            set
            {
                Set(ref _eKeyEff, value);
                _rsaAsymmetricalAlModel.KeyEnDe = KeyState;
            }
           
        }

        public void SwapKeyR()
        {
            if (ButtonEffect == Effect.Decrypt) SwapEnDecrypt();
            switch (KeyState)
            {
                case EKeyEff.Public:
                    KeyState = EKeyEff.Private;
                    (KeyTextAl1, KeyTextAl2) = (KeyTextAl2, KeyTextAl1);
                    KeyEncState = L("PrivateKey") + "!";
                    EncryptTextAl = "";
                    TextNonEncrypt = "";
                    SwapEnDecrypt();
                    break;
                case EKeyEff.Private:
                    KeyState = EKeyEff.Public;
                    (KeyTextAl1, KeyTextAl2) = (KeyTextAl2, KeyTextAl1);
                    EncryptTextAl = "";
                    KeyEncState = L("PublicKey") + "!";
                    
                    TextNonEncrypt = "";
                    break;
                default:
                    KeyState = EKeyEff.Public;
                    break;
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
                    ButtonEDecrypt = L("Decrypt");
                    TextNonEncrypt = EncryptTextAl;
                    EncryptTextAl = "";
                    break;
                case Effect.Decrypt:
                    ButtonEffect = Effect.Encrypt;
                    ButtonEDecrypt = L("Encrypt");
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
        public ICommand AboutProgramCommand { get; }

        private static bool CanCloseAppCommandExecuted() => true;

        public void OnCloseAppCommandExecuted()
        {
            Application.Current.Shutdown();
        }

        private void ShowAboutProgram()
        {
            LocalizationViewModel localization = null;
            try
            {
                localization = SimpleIoc.Default.GetInstance<LocalizationViewModel>();
            }
            catch
            {
                localization = null;
            }

            MessageBox.Show(
                localization?.AboutProgramMessage ?? "EDKGC",
                localization?.AboutProgram ?? "About Program",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
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
            set => Set(ref _title, value);
        }

        #endregion


        #region StatusB

        /// <summary>
        /// Message status bar
        /// </summary>

        private string _status = "Ready";

        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }

        private double _operationProgress;

        public double OperationProgress
        {
            get => _operationProgress;
            set => Set(ref _operationProgress, value);
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

        public ICommand SwapKeyCommand { get; set; }

        public AesSymmetricEncryption AesSymmetricEncryptionM { get; set; }

        public DESsymmetricEncryption DesSymmetricEncryptionM { get; set; }

        public TripleDesSymmetricEncryption TripleDesSymmetricEncryptionM { get; set; }

        public BlowfishSymmetricEncryption BlowfishSymmetricEncryptionM { get; set; }

        public TwofishSymmetricEncryption TwofishSymmetricEncryptionM { get; set; }

        public SerpentSymmetricEncryption SerpentSymmetricEncryptionM { get; set; }

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
                    ConvertByteStringContainer = GetHexModString.GetHexModToString(AesSymmetricEncryptionM.Key);
                    //ConvertByteStringContainer = _encoding.GetString(AesSymmetricEncryptionM.Key);
                    //ConvertByteStringContainer = BitConverter.ToString(AesSymmetricEncryptionM.Key).Replace("-", " ");
                    KeyTextAl = ConvertByteStringContainer;
                    break;
                }
                case SymmetricEncryption.DES:
                {
                    var key = DesSymmetricEncryptionM.GenKeyDes();
                    KeyTextAl = GetHexModString.GetHexModToString(key);
                    //KeyTextAl = _encoding.GetString(key);
                    break;
                }
                case SymmetricEncryption.TripleDES:
                {
                    var key = TripleDesSymmetricEncryptionM.GenKey();
                    KeyTextAl = GetHexModString.GetHexModToString(key);
                    break;
                }
                case SymmetricEncryption.SEAL:
                {
                    KeyTextAl = "SEAL is not supported";
                    break;
                }
                case SymmetricEncryption.Blowfish:
                {
                    var key = BlowfishSymmetricEncryptionM.GenKey();
                    KeyTextAl = GetHexModString.GetHexModToString(key);
                    break;
                }
                case SymmetricEncryption.Twofish:
                {
                    var key = TwofishSymmetricEncryptionM.GenKey();
                    KeyTextAl = GetHexModString.GetHexModToString(key);
                    break;
                }
                case SymmetricEncryption.Serpent:
                {
                    var key = SerpentSymmetricEncryptionM.GenKey();
                    KeyTextAl = GetHexModString.GetHexModToString(key);
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
            if (ButtonEffect == Effect.Encrypt && string.IsNullOrWhiteSpace(TextNonEncrypt))
            {
                TextNonEncrypt = L("EnterText");
                EncryptTextAl = "";
                return;
            }

            switch (_algorithmSymmetricEncryption)
            {
                case SymmetricEncryption.Aes:
                {
                    

                    switch (ButtonEffect)
                    {
                        case Effect.Encrypt:
                            if (TextNonEncrypt != null)
                            {
                                    // ConvertByteStringContainer =
                                    //    _encoding.GetString(AesSymmetricEncryptionM.Encrypting(TextNonEncrypt));
                                    ConvertByteStringContainer =
                                        GetHexModString.GetHexModToString(AesSymmetricEncryptionM.Encrypting(TextNonEncrypt));
                                    EncryptTextAl = ConvertByteStringContainer;
                                TextNonEncrypt = AesSymmetricEncryptionM.EnterText;
                            }
                            else TextNonEncrypt = L("EnterText");
                            
                            break;
                        case Effect.Decrypt:
                            var res = AesSymmetricEncryptionM.Decrypt(TextNonEncrypt);
                            if (res == null) EncryptTextAl = L("InappropriateKey");
                            else
                            {
                                ConvertByteStringContainer = _encoding.GetString(res);
                               // ConvertByteStringContainer = GetHexModString.GetHexModToString(res);
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
                                // ConvertByteStringContainer = _encoding.GetString(DesSymmetricEncryptionM.GetEncryptTextEdc(TextNonEncrypt));
                                ConvertByteStringContainer = GetHexModString.GetHexModToString(DesSymmetricEncryptionM.GetEncryptTextEdc(TextNonEncrypt));

                                EncryptTextAl = ConvertByteStringContainer;
                            TextNonEncrypt = DesSymmetricEncryptionM.EnterText;
                            break;
                        case Effect.Decrypt:
                            ConvertByteStringContainer = DesSymmetricEncryptionM.GetDecryptTextEbc(TextNonEncrypt) ?? L("InappropriateKey");
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
                    EncryptDecryptBouncyBlockCipher(TripleDesSymmetricEncryptionM);
                    break;
                }
                case SymmetricEncryption.SEAL:
                {
                    EncryptTextAl = L("SealNotSupported");
                    break;
                }
                case SymmetricEncryption.Blowfish:
                {
                    EncryptDecryptBouncyBlockCipher(BlowfishSymmetricEncryptionM);
                    break;
                }
                case SymmetricEncryption.Twofish:
                {
                    EncryptDecryptBouncyBlockCipher(TwofishSymmetricEncryptionM);
                    break;
                }
                case SymmetricEncryption.Serpent:
                {
                    EncryptDecryptBouncyBlockCipher(SerpentSymmetricEncryptionM);
                    break;
                }
                default:
                {
                    break;
                }
            }

            


        }

        private void EncryptDecryptBouncyBlockCipher(BouncyBlockSymmetricEncryption encryptionModel)
        {
            switch (ButtonEffect)
            {
                case Effect.Encrypt:
                    if (string.IsNullOrWhiteSpace(TextNonEncrypt))
                    {
                        TextNonEncrypt = L("EnterText");
                        EncryptTextAl = "";
                        return;
                    }

                    ConvertByteStringContainer =
                        GetHexModString.GetHexModToString(encryptionModel.GetEncryptTextEdc(TextNonEncrypt));
                    EncryptTextAl = ConvertByteStringContainer;
                    TextNonEncrypt = encryptionModel.EnterText;
                    break;
                case Effect.Decrypt:
                    ConvertByteStringContainer = encryptionModel.GetDecryptTextEbc(TextNonEncrypt) ?? L("InappropriateKey");
                    EncryptTextAl = ConvertByteStringContainer;
                    break;
                case Effect.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        




    




        #endregion

        #region ASYMMETRIC ALGORITHMS
        /// <summary>
        /// ASYMMETRIC ALGORITHMS
        /// </summary>

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


        private string _keyEncState = "Public Key!";

        public string KeyEncState
        {
            get => _keyEncState;
            set => Set(ref _keyEncState, value);
        }

        private void GenKeyAs()
        {
            StartOperation("Generating asymmetric key...");

            switch (_asymmetricAlgorithms)
            {
                case AsymmetricAlgorithms.RSA:
                    _rsaAsymmetricalAlModel.GenerateKeysRsa();
                    KeyTextAl2 = GetHexModString.GetHexModToString(_rsaAsymmetricalAlModel.GenPrivateKey());
                    KeyTextAl1 = GetHexModString.GetHexModToString(_rsaAsymmetricalAlModel.GenPublicKey());
                    _electronicSignatureRsa.SetKeyPair(_rsaAsymmetricalAlModel.GetKeyP());
                    break;
                case AsymmetricAlgorithms.DiffieHellman:
                    _diffieHellmanAsymmetricalAlModel.GenerateKeys();
                    KeyTextAl1 = GetHexModString.GetHexModToString(_diffieHellmanAsymmetricalAlModel.GetPublicKey());
                    KeyTextAl2 = GetHexModString.GetHexModToString(_diffieHellmanAsymmetricalAlModel.GetPrivateKey());
                    EncryptTextAl = L("SharedSecret") + ": " + GetHexModString.GetHexModToString(_diffieHellmanAsymmetricalAlModel.GetSharedSecret());
                    break;
                case AsymmetricAlgorithms.ElGamal:
                    _elGamalAsymmetricalAlModel.GenerateKeys();
                    KeyTextAl1 = GetHexModString.GetHexModToString(_elGamalAsymmetricalAlModel.GetPublicKey());
                    KeyTextAl2 = GetHexModString.GetHexModToString(_elGamalAsymmetricalAlModel.GetPrivateKey());
                    break;
                case AsymmetricAlgorithms.ECC:
                    _eccAsymmetricalAlModel.GenerateKeys();
                    KeyTextAl1 = GetHexModString.GetHexModToString(_eccAsymmetricalAlModel.GetPublicKey());
                    KeyTextAl2 = GetHexModString.GetHexModToString(_eccAsymmetricalAlModel.GetPrivateKey());
                    EncryptTextAl = L("SharedSecret") + ": " + GetHexModString.GetHexModToString(_eccAsymmetricalAlModel.GetSharedSecret());
                    break;
                default:
                    break;
            }

            CompleteOperation("Asymmetric key ready");
        }

       


        private void EncryptDecryptAs()
        {
            StartOperation(ButtonEffect == Effect.Encrypt ? "Encrypting..." : "Decrypting...");

            if (ButtonEffect == Effect.Encrypt && string.IsNullOrWhiteSpace(TextNonEncrypt))
            {
                TextNonEncrypt = L("EnterText");
                EncryptTextAl = "";
                CompleteOperation("Waiting for text");
                return;
            }

            switch (_asymmetricAlgorithms)
            {
                
                case AsymmetricAlgorithms.RSA:
                {
                    switch (ButtonEffect)
                    {
                        case Effect.Encrypt:
                            if (TextNonEncrypt != null)
                            {
                                    var encryptedText = _rsaAsymmetricalAlModel.EncryptTextRsa(TextNonEncrypt);
                                    if (encryptedText == null)
                                    {
                                        EncryptTextAl = L("RsaTextTooLong");
                                        break;
                                    }
                                    ConvertByteStringContainer = GetHexModString.GetHexModToString(encryptedText);
                                    EncryptTextAl = ConvertByteStringContainer;
                                
                            }
                            else TextNonEncrypt = L("EnterText");

                            break;
                        case Effect.Decrypt:
                            try
                            {
                                var res = _rsaAsymmetricalAlModel.DecryptTextRsa(GetHexModString.GetStringToHexMod(TextNonEncrypt));
                                if (res == null) EncryptTextAl = L("InappropriateKeyOrText");
                                else
                                {
                                    ConvertByteStringContainer = res;
                                    EncryptTextAl = ConvertByteStringContainer;
                                }
                            }
                            catch (ArgumentException)
                            {
                                EncryptTextAl = L("InappropriateKeyOrText");
                            }

                            break;
                    }


                    }
                    break;
                case AsymmetricAlgorithms.DiffieHellman:
                {
                    EncryptDecryptSharedSecretAsymmetric(_diffieHellmanAsymmetricalAlModel);
                }
                    break;
                case AsymmetricAlgorithms.ElGamal:
                {
                    EncryptDecryptElGamal();
                }
                    break;
                case AsymmetricAlgorithms.ECC:
                {
                    EncryptDecryptSharedSecretAsymmetric(_eccAsymmetricalAlModel);
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            CompleteOperation("Ready");
        }

        private void EncryptDecryptSharedSecretAsymmetric(ISharedSecretAsymmetricalAlModel encryptionModel)
        {
            try
            {
                switch (ButtonEffect)
                {
                    case Effect.Encrypt:
                        ConvertByteStringContainer =
                            GetHexModString.GetHexModToString(encryptionModel.EncryptText(TextNonEncrypt));
                        EncryptTextAl = ConvertByteStringContainer;
                        break;
                    case Effect.Decrypt:
                        ConvertByteStringContainer =
                            encryptionModel.DecryptText(GetHexModString.GetStringToHexMod(TextNonEncrypt));
                        EncryptTextAl = ConvertByteStringContainer;
                        break;
                    case Effect.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception)
            {
                EncryptTextAl = L("InappropriateKeyOrText");
            }
        }

        private void EncryptDecryptElGamal()
        {
            try
            {
                switch (ButtonEffect)
                {
                    case Effect.Encrypt:
                        ConvertByteStringContainer =
                            GetHexModString.GetHexModToString(_elGamalAsymmetricalAlModel.EncryptText(TextNonEncrypt));
                        EncryptTextAl = ConvertByteStringContainer;
                        break;
                    case Effect.Decrypt:
                        ConvertByteStringContainer =
                            _elGamalAsymmetricalAlModel.DecryptText(GetHexModString.GetStringToHexMod(TextNonEncrypt));
                        EncryptTextAl = ConvertByteStringContainer;
                        break;
                    case Effect.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception)
            {
                EncryptTextAl = L("InappropriateKeyOrText");
            }
        }

        #endregion

        #region Signature

        /// <summary>
        /// Signature
        /// </summary>
        private ElectronicSignatureRSA _electronicSignatureRsa;

        public ICommand ButGenKeyPCommand { get; set; }
        public ICommand GenHashAndEncryptSignatureCommand { get; set; }
        public ICommand DecryptEncryptHashCommand { get; set; }
        public ICommand VerifySignatureCommand { get; set; }

        private string _enterTextS = "Enter text";

        private string _hashEntTextS = "Expect hash";

        private string _textResp = "...";

        private byte[] hashNotEncrypt { get; set; }

        private byte[] enctyptTextBytes { get; set; }
        private string _signatureText;
        private bool _isSignatureDecrypted;
        private string _signatureDecryptButtonText = "Decrypt";



        public string TextResp
        {
            get =>   _textResp;
            set => Set(ref _textResp, value);
        }

        public string HashEntTextS
        {
            get => _hashEntTextS;
            set => Set(ref _hashEntTextS, value);
        }

        public string EnterTextS
        {
            get => _enterTextS;
            set => Set(ref _enterTextS, value);
        }

        private string _encryptVerTextBoxS = "Expect Encrypt";

        public string EncryptVerTextBoxS
        {
            get=> _encryptVerTextBoxS;
            set=> Set(ref _encryptVerTextBoxS, value);
        }

        public string SignatureDecryptButtonText
        {
            get => _signatureDecryptButtonText;
            set => Set(ref _signatureDecryptButtonText, value);
        }




        private void GenKeyPeir()
        {
            StartOperation("Generating signature keys...");

            _rsaAsymmetricalAlModel.GenerateKeysRsa();
            KeyTextAl2 = GetHexModString.GetHexModToString(_rsaAsymmetricalAlModel.GenPrivateKey());
            KeyTextAl1 = GetHexModString.GetHexModToString(_rsaAsymmetricalAlModel.GenPublicKey());
            _electronicSignatureRsa.SetKeyPair(_rsaAsymmetricalAlModel.GetKeyP());
            _signatureText = null;
            _isSignatureDecrypted = false;
            SignatureDecryptButtonText = L("Decrypt");
            EncryptVerTextBoxS = L("ExpectEncrypt");
            HashEntTextS = L("ExpectHash");
            TextResp = "...";
            CompleteOperation("Signature keys ready");

        }

        private void GenSignature()
        {
            StartOperation("Generating signature...");

            HashEntTextS = _electronicSignatureRsa.GenHashPrivKey(EnterTextS);
            hashNotEncrypt = _electronicSignatureRsa.GetHashBytes();
            EncryptVerTextBoxS = _electronicSignatureRsa.EncryptHashText(EnterTextS);
            if (EncryptVerTextBoxS == null)
            {
                EncryptVerTextBoxS = L("CouldNotGenerateSignature");
                CompleteOperation("Signature failed");
                return;
            }
            _signatureText = EncryptVerTextBoxS;
            _isSignatureDecrypted = false;
            SignatureDecryptButtonText = L("Decrypt");
            enctyptTextBytes = GetHexModString.GetStringToHexMod(EncryptVerTextBoxS);
            TextResp = "...";
            CompleteOperation("Signature generated");
        }

        private void DecryptTextHashS()
        {
            if (_isSignatureDecrypted)
            {
                StartOperation("Restoring signature...");
                if (string.IsNullOrEmpty(_signatureText))
                {
                    EncryptVerTextBoxS = L("GenerateSignatureFirst");
                    CompleteOperation("Signature missing");
                    return;
                }

                EncryptVerTextBoxS = _signatureText;
                _isSignatureDecrypted = false;
                SignatureDecryptButtonText = L("Decrypt");
                CompleteOperation("Signature restored");
                return;
            }

            StartOperation("Decrypting signature...");

            var signature = GetSignatureForVerification();
            if (signature == null)
            {
                EncryptVerTextBoxS = L("GenerateSignatureFirst");
                CompleteOperation("Signature missing");
                return;
            }

            var signatureBytes = GetHexModString.GetStringToHexMod(signature);
            EncryptVerTextBoxS = _electronicSignatureRsa.DecryptTextHash(signatureBytes) ?? L("InappropriateSignature");
            _isSignatureDecrypted = EncryptVerTextBoxS != L("InappropriateSignature");
            SignatureDecryptButtonText = _isSignatureDecrypted ? L("Encrypt") : L("Decrypt");
            CompleteOperation("Signature decrypted");
        }
        private SolidColorBrush _textColor = Brushes.Green;
        public SolidColorBrush TextColor
        {
            get { return _textColor; }
            set
            {
                if (_textColor != value)
                {
                    _textColor = value;
                    OnPropertyChanged();
                }
            }
        }

        private void VerifySignRsa()
        {
            StartOperation("Verifying signature...");

            var signatureForVerify = GetSignatureForVerification();
            if (signatureForVerify == null)
            {
                TextResp = L("Hazard");
                TextColor = Brushes.Red;
                CompleteOperation("Signature missing");
                return;
            }

            TextResp = _electronicSignatureRsa.VerifySignature(HashEntTextS, signatureForVerify);
            TextColor = TextResp == "Pass" ? Brushes.Green : Brushes.Red;
            TextResp = TextResp == "Pass" ? L("Pass") : L("Hazard");
            CompleteOperation(TextResp == L("Pass") ? "Signature verified" : "Signature mismatch");
        }

        private string GetSignatureForVerification()
        {
            if (EncryptVerTextBoxS == HashEntTextS && !string.IsNullOrEmpty(_signatureText))
                return _signatureText;

            try
            {
                GetHexModString.GetStringToHexMod(EncryptVerTextBoxS);
                return EncryptVerTextBoxS;
            }
            catch (ArgumentException)
            {
                return string.IsNullOrEmpty(_signatureText) ? null : _signatureText;
            }
        }

        private void StartOperation(string status)
        {
            Status = status;
            OperationProgress = 20;
        }

        private void CompleteOperation(string status)
        {
            OperationProgress = 100;
            Status = status;
        }

        #endregion

    }
}
