using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EDKGC.Infrastructure.Command;
using Newtonsoft.Json;

namespace EDKGC.ViewModel
{
    public class LocalizationViewModel : INotifyPropertyChanged
    {
        private const string EnglishCode = "en";
        private const string RussianCode = "ru";

        private bool _isEnglish = true;
        private Dictionary<string, string> _strings = new();

        public LocalizationViewModel()
        {
            ToggleLanguageCommand = new LCommand(_ => ToggleLanguage());
            LoadStrings();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ToggleLanguageCommand { get; }

        public bool IsEnglish
        {
            get => _isEnglish;
            private set
            {
                if (_isEnglish == value) return;
                _isEnglish = value;
                LoadStrings();
                OnPropertyChanged();
                OnPropertyChanged(string.Empty);
            }
        }

        public string LanguageCode => IsEnglish ? EnglishCode : RussianCode;

        public string this[string key] => Text(key);

        public string Text(string key)
        {
            return _strings.TryGetValue(key, out var value) ? value : key;
        }

        private void LoadStrings()
        {
            string fileName = IsEnglish ? "ui.en.json" : "ui.ru.json";
            string path = Path.Combine(AppContext.BaseDirectory, "Data", "Localization", fileName);

            if (!System.IO.File.Exists(path))
            {
                _strings = new Dictionary<string, string>();
                return;
            }

            string json = System.IO.File.ReadAllText(path);
            _strings = JsonConvert.DeserializeObject<Dictionary<string, string>>(json)
                       ?? new Dictionary<string, string>();
        }

        public string File => Text(nameof(File));
        public string Helper => Text(nameof(Helper));
        public string BackToHome => Text(nameof(BackToHome));
        public string Exit => Text(nameof(Exit));
        public string AboutProgram => Text(nameof(AboutProgram));
        public string LanguageToggleText => Text(nameof(LanguageToggleText));
        public string Start => Text(nameof(Start));
        public string Open => Text(nameof(Open));
        public string AppSubtitle => Text(nameof(AppSubtitle));
        public string CryptographyWorkspace => Text(nameof(CryptographyWorkspace));
        public string WorkspaceSuffix => Text(nameof(WorkspaceSuffix));
        public string CentralWorkspace => Text(nameof(CentralWorkspace));
        public string CentralWorkspaceDescription => Text(nameof(CentralWorkspaceDescription));
        public string IsoDescription => Text(nameof(IsoDescription));
        public string BasicInformation => Text(nameof(BasicInformation));
        public string BasicInformationDescription => Text(nameof(BasicInformationDescription));
        public string SymmetricEncryption => Text(nameof(SymmetricEncryption));
        public string AsymmetricEncryption => Text(nameof(AsymmetricEncryption));
        public string Signing => Text(nameof(Signing));
        public string Algorithms => Text(nameof(Algorithms));
        public string InputAndKey => Text(nameof(InputAndKey));
        public string GenerateKey => Text(nameof(GenerateKey));
        public string GenerateKeys => Text(nameof(GenerateKeys));
        public string GenerateSignature => Text(nameof(GenerateSignature));
        public string Result => Text(nameof(Result));
        public string Swap => Text(nameof(Swap));
        public string Copy => Text(nameof(Copy));
        public string SignatureInput => Text(nameof(SignatureInput));
        public string SignatureMethod => Text(nameof(SignatureMethod));
        public string Hash => Text(nameof(Hash));
        public string SignatureDecryptedHash => Text(nameof(SignatureDecryptedHash));
        public string Verification => Text(nameof(Verification));
        public string Verify => Text(nameof(Verify));
        public string Decrypt => Text(nameof(Decrypt));
        public string Encrypt => Text(nameof(Encrypt));
        public string PublicKey => Text(nameof(PublicKey));
        public string PrivateKey => Text(nameof(PrivateKey));
        public string GenerateKeyPrompt => Text(nameof(GenerateKeyPrompt));
        public string EnterText => Text(nameof(EnterText));
        public string ExpectHash => Text(nameof(ExpectHash));
        public string ExpectEncrypt => Text(nameof(ExpectEncrypt));
        public string Pass => Text(nameof(Pass));
        public string Hazard => Text(nameof(Hazard));
        public string InappropriateKey => Text(nameof(InappropriateKey));
        public string InappropriateKeyOrText => Text(nameof(InappropriateKeyOrText));
        public string InappropriateSignature => Text(nameof(InappropriateSignature));
        public string GenerateSignatureFirst => Text(nameof(GenerateSignatureFirst));
        public string CouldNotGenerateSignature => Text(nameof(CouldNotGenerateSignature));
        public string RsaTextTooLong => Text(nameof(RsaTextTooLong));
        public string SealNotSupported => Text(nameof(SealNotSupported));
        public string SharedSecret => Text(nameof(SharedSecret));
        public string CurrentQuestion => Text(nameof(CurrentQuestion));
        public string Yes => Text(nameof(Yes));
        public string No => Text(nameof(No));
        public string DoNotKnow => Text(nameof(DoNotKnow));
        public string ThreatLevel => Text(nameof(ThreatLevel));
        public string Recommendation => Text(nameof(Recommendation));
        public string Info => Text(nameof(Info));
        public string RiskAnalysis => Text(nameof(RiskAnalysis));
        public string ThreatEvent => Text(nameof(ThreatEvent));
        public string RateOfOccurrence => Text(nameof(RateOfOccurrence));
        public string ThreatLevels => Text(nameof(ThreatLevels));
        public string Recommendations => Text(nameof(Recommendations));
        public string RiskTotalPrefix => Text(nameof(RiskTotalPrefix));
        public string IsoOverview => Text(nameof(IsoOverview));
        public string OccurrenceGlue => Text(nameof(OccurrenceGlue));
        public string Years => Text(nameof(Years));
        public string Months => Text(nameof(Months));
        public string Low => Text(nameof(Low));
        public string Medium => Text(nameof(Medium));
        public string High => Text(nameof(High));
        public string Critical => Text(nameof(Critical));
        public string Safely => Text(nameof(Safely));
        public string None => Text(nameof(None));
        public string BasicInfoMessage => Text(nameof(BasicInfoMessage));
        public string ConfirmLeaveTitle => Text(nameof(ConfirmLeaveTitle));
        public string ConfirmLeaveText => Text(nameof(ConfirmLeaveText));
        public string ComeBack => Text(nameof(ComeBack));
        public string AboutProgramMessage => Text(nameof(AboutProgramMessage));
        public string Pending => Text(nameof(Pending));
        public string IsoOverviewText => Text(nameof(IsoOverviewText));
        public string IsoInfoText => Text(nameof(IsoInfoText));
        public string ResultPercentFormat => Text(nameof(ResultPercentFormat));

        private void ToggleLanguage()
        {
            IsEnglish = !IsEnglish;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
