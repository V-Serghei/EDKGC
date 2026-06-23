using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EDKGC.Infrastructure.Command;

namespace EDKGC.ViewModel
{
    public class LocalizationViewModel : INotifyPropertyChanged
    {
        private bool _isEnglish = true;

        private readonly Dictionary<string, (string Ru, string En)> _strings = new()
        {
            ["File"] = ("Файл", "File"),
            ["Helper"] = ("Помощь", "Helper"),
            ["BackToHome"] = ("На главную", "Back to home"),
            ["Exit"] = ("Выйти", "Exit"),
            ["AboutProgram"] = ("О программе", "About Program"),
            ["LanguageToggleText"] = ("English", "Русский"),
            ["Start"] = ("Старт", "Start"),
            ["Open"] = ("Открыть", "Open"),
            ["AppSubtitle"] = ("Шифратор / Дешифратор / Генератор ключей / Анализ рисков", "Encryptor / Decryptor / Key Generator / Risk Analyzer"),
            ["CryptographyWorkspace"] = ("Рабочая область криптографии", "Cryptography workspace"),
            ["WorkspaceSuffix"] = (" рабочая область", " workspace"),
            ["CentralWorkspace"] = ("Центральная рабочая область", "Central workspace"),
            ["CentralWorkspaceDescription"] = ("Шифрование, RSA и электронная подпись.", "Open encryption, RSA, and signature tools."),
            ["IsoDescription"] = ("Оценка контролей и расчет рисков.", "Answer controls and estimate risk exposure."),
            ["BasicInformation"] = ("Основная информация", "Basic information"),
            ["BasicInformationDescription"] = ("Справочная информация о назначении приложения.", "Documentation area reserved for the next iteration."),
            ["SymmetricEncryption"] = ("Симметричное шифрование", "Symmetric encryption"),
            ["AsymmetricEncryption"] = ("Асимметричное шифрование", "Asymmetric encryption"),
            ["Signing"] = ("Подпись", "Signing"),
            ["Algorithms"] = ("Алгоритмы", "Algorithms"),
            ["InputAndKey"] = ("Ввод и ключ", "Input and key"),
            ["GenerateKey"] = ("Сгенерировать ключ", "Generate key"),
            ["GenerateKeys"] = ("Сгенерировать ключи", "Generate keys"),
            ["GenerateSignature"] = ("Сгенерировать подпись", "Generate signature"),
            ["Result"] = ("Результат", "Result"),
            ["Swap"] = ("Поменять", "Swap"),
            ["Copy"] = ("Копировать", "Copy"),
            ["SignatureInput"] = ("Данные подписи", "Signature input"),
            ["SignatureMethod"] = ("Метод: SHA-256 хеш, RSA/PKCS#1 подпись приватным ключом, проверка публичным ключом.", "Method: SHA-256 hash, RSA/PKCS#1 signature with private key, verification with public key."),
            ["Hash"] = ("Хеш", "Hash"),
            ["SignatureDecryptedHash"] = ("Подпись / расшифрованный хеш", "Signature / decrypted hash"),
            ["Verification"] = ("Проверка", "Verification"),
            ["Verify"] = ("Проверить", "Verify"),
            ["Decrypt"] = ("Расшифровать", "Decrypt"),
            ["Encrypt"] = ("Шифровать", "Encrypt"),
            ["PublicKey"] = ("Публичный ключ", "Public Key"),
            ["PrivateKey"] = ("Приватный ключ", "Private Key"),
            ["GenerateKeyPrompt"] = ("Сгенерируйте ключ", "Generate Key"),
            ["EnterText"] = ("Введите текст", "Enter text"),
            ["ExpectHash"] = ("Ожидается хеш", "Expect hash"),
            ["ExpectEncrypt"] = ("Ожидается подпись", "Expect Encrypt"),
            ["Pass"] = ("Пройдено", "Pass"),
            ["Hazard"] = ("Ошибка!!!", "Hazard!!!"),
            ["InappropriateKey"] = ("Неверный ключ", "Inappropriate key"),
            ["InappropriateKeyOrText"] = ("Неверный ключ или текст", "Inappropriate key or text"),
            ["InappropriateSignature"] = ("Неверная подпись", "Inappropriate signature"),
            ["GenerateSignatureFirst"] = ("Сначала сгенерируйте подпись", "Generate signature first"),
            ["CouldNotGenerateSignature"] = ("Не удалось сгенерировать подпись", "Could not generate signature"),
            ["RsaTextTooLong"] = ("Текст слишком длинный для RSA. Используйте короткое сообщение или шифруйте хеш/ключ файла.", "RSA text is too long. Use a shorter message or encrypt a hash/file key."),
            ["SealNotSupported"] = ("SEAL не поддерживается", "SEAL is not supported"),
            ["SharedSecret"] = ("Общий секрет", "Shared secret"),
            ["CurrentQuestion"] = ("Текущий вопрос", "Current question"),
            ["Yes"] = ("Да", "Yes"),
            ["No"] = ("Нет", "No"),
            ["DoNotKnow"] = ("Не знаю", "Do not know"),
            ["ThreatLevel"] = ("Уровень угрозы", "Threat level"),
            ["Recommendation"] = ("Рекомендация", "Recommendation"),
            ["Info"] = ("Инфо", "Info"),
            ["RiskAnalysis"] = ("Анализ риска", "Risk Analysis"),
            ["ThreatEvent"] = ("Событие угрозы", "Threat Event"),
            ["RateOfOccurrence"] = ("Частота возникновения", "Rate of Occurrence"),
            ["ThreatLevels"] = ("Уровни угроз", "Threat levels"),
            ["Recommendations"] = ("Рекомендации", "Recommendations"),
            ["RiskTotalPrefix"] = ("Результат подсчета, возможные общие потери: $", "Estimated total loss exposure: $"),
            ["IsoOverview"] = ("Обзор ISO 27001", "ISO 27001 overview"),
            ["OccurrenceGlue"] = ("раз в", "times per"),
            ["Years"] = ("лет", "years"),
            ["Months"] = ("месяцев", "months"),
            ["Low"] = ("Низкий", "Low"),
            ["Medium"] = ("Средний", "Medium"),
            ["High"] = ("Высокий", "High"),
            ["Critical"] = ("Критический", "Critical"),
            ["Safely"] = ("Безопасно", "Safely"),
            ["None"] = ("Нет", "None"),
            ["BasicInfoMessage"] = ("Модули EDKGC:\n\nСимметричное шифрование: AES, DES, 3DES, Blowfish, Twofish, Serpent.\nАсимметричное шифрование: RSA, Diffie-Hellman, ElGamal и ECC.\nПодпись: SHA-256, RSA/PKCS#1 подпись, расшифровка и проверка.\nISO 27001: анкета, рекомендации и таблица анализа риска.\n\nПримечание: RSA напрямую шифрует только короткие сообщения. Для длинных данных используйте хеш или симметричный ключ.", "EDKGC modules:\n\nSymmetric encryption: AES, DES, 3DES, Blowfish, Twofish, Serpent.\nAsymmetric encryption: RSA, Diffie-Hellman, ElGamal, and ECC.\nSigning: SHA-256, RSA/PKCS#1 signature, decrypt, and verify flow.\nISO 27001: questionnaire, recommendations, and risk analysis table.\n\nNote: RSA can encrypt only short messages directly. For long data, encrypt a hash or a symmetric key."),
            ["ConfirmLeaveTitle"] = ("Вы уверены, что хотите выйти?", "Are you sure you want to leave?"),
            ["ConfirmLeaveText"] = ("Несохраненные данные текущего процесса могут быть потеряны.", "Unsaved data in the current workflow may be lost."),
            ["ComeBack"] = ("Вернуться", "Come back"),
            ["AboutProgramMessage"] = ("EDKGC\n\nУчебное настольное приложение для криптографии и анализа рисков ISO 27001.\n\nМодули:\n- Симметричное шифрование: AES, DES, 3DES, Blowfish, Twofish, Serpent.\n- Асимметричное шифрование: RSA, Diffie-Hellman, ElGamal, ECC.\n- Подпись: SHA-256, RSA/PKCS#1 подпись приватным ключом, проверка публичным ключом.\n- ISO 27001: анкета, уровни угроз, рекомендации и таблица ALE.\n\nПримечания:\n- RSA напрямую шифрует только короткие сообщения.\n- Diffie-Hellman и ECC формируют общий секрет, затем приложение использует AES для текста.", "EDKGC\n\nDesktop training tool for cryptography and ISO 27001 risk analysis.\n\nModules:\n- Symmetric encryption: AES, DES, 3DES, Blowfish, Twofish, Serpent.\n- Asymmetric encryption: RSA, Diffie-Hellman, ElGamal, ECC.\n- Signing: SHA-256 hash, RSA/PKCS#1 signature with private key, verification with public key.\n- ISO 27001: questionnaire, threat levels, recommendations, and ALE risk table.\n\nNotes:\n- RSA directly encrypts only short messages.\n- Diffie-Hellman and ECC derive a shared secret, then this app uses AES for text encryption."),
        };

        public LocalizationViewModel()
        {
            ToggleLanguageCommand = new LCommand(_ => ToggleLanguage());
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
                OnPropertyChanged();
                OnPropertyChanged(string.Empty);
            }
        }

        public string this[string key] => Text(key);

        public string Text(string key)
        {
            if (!_strings.TryGetValue(key, out var pair)) return key;
            return IsEnglish ? pair.En : pair.Ru;
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
