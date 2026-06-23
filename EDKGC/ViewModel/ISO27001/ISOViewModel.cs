using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EDKGC.Enams;
using EDKGC.Infrastructure.Command;
using EDKGC.Infrastructure.Command.BasicCommands;
using EDKGC.Models.ISO27001;
using EDKGC.ViewModel.SatelliteWindows;
using GalaSoft.MvvmLight.Ioc;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;
using ViewModelBase = GalaSoft.MvvmLight.ViewModelBase;

namespace EDKGC.ViewModel.ISO27001
{
    public class ISOViewModel: ViewModelBase, INotifyPropertyChanged
    {
        #region CONNECTION AND CONTROL AREA

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

        #region Main tools

        public List<QuestionModel> Question { get; set; }

        public Answer CurrAnswer { get; set; }

        public int CurrentIndex = 0;

        public QuestionModel CurrentQuestion { get; set; }

        public SeriesCollection ChartSeries { get; set; }

        public SeriesCollection ChartResult { get; set; }


        public PieChartData SeriesData { get; set; }

        public CharResultData CharResultDataM { get; set; }
        public ICommand OpenIsoResultsWindowCommand { get; }
        private readonly LocalizationViewModel _localization;

        private static readonly Dictionary<int, (string Description, string Title, string RespPos, string RespNeg)> EnglishQuestions = new()
        {
            [1] = ("Is the information security policy approved, understood by employees, and reviewed regularly?", "Information security policy", "The information security policy is defined, approved, and maintained. This is a strong baseline control for the ISMS.", "Create and approve an information security policy, communicate it to employees, and set a regular review cycle. Without this baseline, other controls remain fragmented."),
            [2] = ("Are information security responsibilities assigned and are conflicting duties separated?", "Roles, responsibilities, and segregation of duties", "Responsibilities are assigned and conflicting duties are separated. This reduces errors, abuse, and unauthorized changes.", "Assign process owners and security responsibilities, and separate conflicting duties so one person cannot perform critical actions without control."),
            [3] = ("Is information security risk assessment performed regularly and are risk treatment decisions recorded?", "Risk assessment and treatment", "Risk assessment is performed regularly and risk treatment decisions are recorded. This matches the risk-based approach of ISO 27001.", "Maintain a risk register, assess likelihood and impact, choose treatment options, and track owners and due dates."),
            [4] = ("Is there an up-to-date inventory of information assets with owners and usage rules?", "Asset inventory and ownership", "Assets are inventoried, owners are assigned, and usage rules are clear. This improves protection and accountability.", "Identify critical assets, assign owners, define usage rules, and update the inventory regularly."),
            [5] = ("Is information classified by sensitivity and handled according to marking and handling rules?", "Information classification", "Information is classified and handled by clear rules. This lowers the risk of leakage and mishandling.", "Define classification levels and rules for marking, storage, transfer, and disposal for each level."),
            [6] = ("Are access rights managed by least privilege and reviewed after role changes?", "Access control", "Access rights are granted by need, reviewed, and revoked on time. This is a key organizational and technical control.", "Implement a process for granting, changing, reviewing, and revoking access, especially when employees transfer or leave."),
            [7] = ("Is multi-factor authentication used for critical systems and remote access?", "Authentication", "MFA is used for critical scenarios. This significantly reduces account compromise risk.", "Enable MFA for administrators, remote access, cloud services, and systems containing critical data."),
            [8] = ("Are approved cryptographic measures used to protect confidential data in storage and transit?", "Cryptographic protection", "Cryptographic controls and key management are defined. This improves protection of data in transit and at rest.", "Define where encryption is required, which algorithms are allowed, how keys are managed, and who owns the key lifecycle."),
            [9] = ("Is there a process for reporting, investigating, and resolving information security incidents?", "Incident management", "The incident management process is defined and used. This helps limit damage faster and prevent recurrence.", "Define incident channels, roles, response times, evidence handling, root-cause analysis, and corrective actions."),
            [10] = ("Are supplier risks assessed and are security requirements included in contracts or agreements?", "Supplier security", "Supplier risks are considered and security requirements are contractually defined. This reduces exposure through third parties.", "Assess suppliers by risk, document security requirements in contracts, and monitor compliance for critical services."),
            [11] = ("Are backup, recovery, and continuity plans defined for critical processes?", "Continuity and recovery", "Recovery and continuity plans are defined and tested. This reduces the impact of outages, attacks, and data loss.", "Define critical processes, RTO/RPO, backup and recovery procedures, and test them regularly."),
            [12] = ("Are internal reviews, management review, and corrective actions performed based on security assessment results?", "Monitoring, audit, and improvement", "The organization reviews and improves the ISMS. This supports the continual improvement cycle.", "Perform internal reviews, record nonconformities, assign corrective actions, and review results with management."),
            [13] = ("Is security awareness training performed and is employee understanding of phishing, passwords, and data handling checked?", "Awareness and training", "Security awareness and training are performed. This reduces user error and social engineering risk.", "Run regular training, phishing exercises, and keep completion evidence for employees and contractors."),
            [14] = ("Are security events logged and are logs from critical systems reviewed?", "Logging and monitoring", "Logging and event review are configured. This supports attack detection, investigations, and accountability.", "Enable access, administration, error, and security logs; define retention periods and review responsibilities."),
            [15] = ("Is there a vulnerability management process covering scanning, prioritization, and remediation tracking?", "Vulnerability management", "Vulnerability management is operating. This lowers the likelihood of known weaknesses being exploited.", "Scan regularly, prioritize findings, assign remediation owners, and track deadlines."),
            [16] = ("Is malware protection used and is the health of protection tools monitored?", "Malware protection", "Malware protection is deployed and monitored. This lowers infection and lateral movement risk.", "Use up-to-date protection tools, monitor updates, detections, and exclusions."),
            [17] = ("Are secure baseline configurations used for servers, workstations, network devices, and cloud services?", "Secure configuration", "Secure baselines are defined and monitored. This reduces the attack surface.", "Define configuration baselines, disable unnecessary services, control changes, and review compliance."),
            [18] = ("Are information transfer channels protected and are rules defined for confidential data exchange?", "Information transfer", "Information transfer is controlled and protected. This reduces leakage risk during data exchange.", "Define approved transfer channels, encryption requirements, recipients, logging, and confirmation rules."),
            [19] = ("Are system changes managed through approval, testing, rollback planning, and recording?", "Change management", "Changes are managed and documented. This reduces outage and unauthorized modification risk.", "Use a change process with request, risk review, testing, rollback plan, and outcome record."),
            [20] = ("Are security requirements considered during software development, testing, and release?", "Secure development", "Security is built into development and testing. This reduces application vulnerability risk.", "Include security requirements, code review, testing, secrets management, and environment separation."),
            [21] = ("Are cloud service risks assessed and are access control, logging, and data protection configured?", "Cloud security", "Cloud services are managed according to risk. This reduces misconfiguration and leakage risk.", "Assess cloud risks and configure MFA, roles, logs, encryption, backups, and supplier responsibilities."),
            [22] = ("Is physical access to premises, equipment, and information media controlled?", "Physical security", "Physical access is controlled. This reduces theft, damage, and unauthorized access risk.", "Define access zones, visitor rules, media handling, equipment protection, and asset removal controls."),
            [23] = ("Are legal, contractual, and regulatory requirements applicable to information and systems identified?", "Legal and contractual requirements", "Legal and contractual requirements are identified and monitored. This reduces breach of obligation and penalty risk.", "Maintain a list of applicable requirements, assign owners, and review compliance regularly."),
            [24] = ("Is the effectiveness of security controls checked through metrics, tests, audits, or incident analysis?", "Control effectiveness", "Control effectiveness is reviewed. This helps verify that controls work in practice, not only on paper.", "Define security metrics, test controls periodically, and use results for improvement."),
        };

        private string _textStart;
        private string _textInfoIso;

        public string TextStart
        {
            get => _textStart;
            set => Set(ref _textStart, value);
        }

        public string TextInfoIso
        {
            get => _textInfoIso;
            set => Set(ref _textInfoIso, value);
        }

        public TextInfo BaseTextInfo { get; }

        public string ResultPercent { get; set; }

        #endregion

        public ISOViewModel()
        {


            Question = new List<QuestionModel>();
            string jsonPath = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "ISOQuestion.json");
            string json = File.ReadAllText(jsonPath);
            Question = JsonConvert.DeserializeObject<List<QuestionModel>>(json);
            OpenIsoResultsWindowCommand = new OpenIsoResultsWindowCommand(this);
            try
            {
                _localization = SimpleIoc.Default.GetInstance<LocalizationViewModel>();
            }
            catch
            {
                _localization = null;
            }

            if (_localization != null)
            {
                _localization.PropertyChanged += (_, _) => RefreshLanguage();
            }
            BaseTextInfo = new TextInfo();
            TextStart = GetIsoOverviewText();
            TextInfoIso = GetIsoInfoText();

            ChartSeries = new SeriesCollection
            {
                new PieSeries
                {
                    Title = _localization?.Yes ?? "Yes",
                    Values = new ChartValues<int>(),
                    Fill = Brushes.Green
                },
                new PieSeries
                {
                    Title = _localization?.No ?? "No",
                    Values = new ChartValues<int>(),
                    Fill = Brushes.Red
                },
                new PieSeries
                {
                    Title = _localization?.DoNotKnow ?? "Do not know",
                    Values = new ChartValues<int>(),
                    Fill = Brushes.Yellow
                },
                new PieSeries
                {
                    Title = IsEnglish ? "Pending" : "Ждет ответа",
                    Values = new ChartValues<int>{Question.Count},
                    Fill = Brushes.Gray
                }
            };

            ChartResult = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Low",
                    Values = new ChartValues<int>(),
                    Fill = Brushes.Yellow
                },
                new PieSeries
                {
                    Title = "Medium",
                    Values = new ChartValues<int>(),
                    Fill = Brushes.Orange
                },
                new PieSeries
                {
                    Title = "High",
                    Values = new ChartValues<int>(),
                    Fill = Brushes.OrangeRed
                },
                new PieSeries
                {
                    Title = "Critical",
                    Values = new ChartValues<int>{Question.Count},
                    Fill = Brushes.DarkRed
                }
            };



            RespYesCommand = new LCommand(ResolveYesCommand);

            RespNoCommand = new LCommand(ResolveNoCommand);

            RespDonTKnowCommand = new LCommand(ResolveDonTKnowCommand);

            CurrentQuestion = Question[CurrentIndex];

            _question = GetDescription(CurrentQuestion);

            CharResultDataM = new CharResultData
            {
                ResLow = 0,
                ResHigh = 0,
                ResCritical = 0,
                ResMedium = 0
            };

            SeriesData = new PieChartData
            {
                RespYes = 0,
                RespNo = 0,
                RespDonTKnow = 0,
                RespNone = Question.Count
            };
            CurrentIndexQuestion = CurrentIndex + "/" + Question.Count;


            UpdateLocalizedCollections();

        }




        #region questions

        private string _question;
        private ICommand _respYesCommand;
        private ICommand _respNoCommand;
        private ICommand _respDonTKnowCommand;
        private int _percentage;
        private string _currIndexQuestion;

        public string CurrentIndexQuestion
        {
            get => _currIndexQuestion;

            set => Set(ref _currIndexQuestion, value);
        }

        public int Percentage
        {
            get => _percentage;
            set =>
                Set(ref _percentage, value);

        }

        public string QuestionCurr
        {
            get => _question;
            set =>
                Set(ref _question, value);

        }

        public ICommand RespYesCommand
        {
            get => _respYesCommand;
            set
            {
                
                Set(ref _respYesCommand, value);
                CurrAnswer = Answer.Yes;
            }
        }

        public ICommand RespNoCommand
        {
            get => _respNoCommand;
            set
            {
                Set(ref _respNoCommand, value);
                CurrAnswer = Answer.No;
            }
        }

        public ICommand RespDonTKnowCommand
        {
            get => _respDonTKnowCommand;
            set
            {
                Set(ref _respDonTKnowCommand, value);
                CurrAnswer = Answer.DonTKnow;

            }
        }

        private void ResolveYesCommand(object obj)
        {
            CurrAnswer = Answer.Yes;
            ResolveQuestion();
        }

        private void ResolveNoCommand(object obj)
        {
            CurrAnswer = Answer.No;
            ResolveQuestion();
        }

        private void ResolveDonTKnowCommand(object obj)
        {
            CurrAnswer = Answer.DonTKnow;
            ResolveQuestion();
        }


        private void ResolveQuestion()
        {
            CurrentQuestion.Resolved = CurrAnswer;
           if(CurrentIndex < Question.Count) {
               Question[CurrentIndex].Resolved = CurrAnswer;

                foreach (var series in ChartSeries)
                {
                    series.Values.Clear();
                }

                


                switch (CurrAnswer)
                {
                    case Answer.Yes:
                        SeriesData.RespYes += 1;
                        SeriesData.RespNone -= 1;
                        break;
                    case Answer.No:
                        SeriesData.RespNo += 1;
                        SeriesData.RespNone -= 1;
                        break;
                    case Answer.DonTKnow:
                        SeriesData.RespDonTKnow += 1;
                        SeriesData.RespNone -= 1;
                        break;
                    default:
                        SeriesData.RespNone -= 1;
                        break;
                }

                if(CurrAnswer == Answer.DonTKnow || CurrAnswer == Answer.No){
                    foreach (var series in ChartResult)
                    {
                        series.Values.Clear();
                    }

                    if (Question[CurrentIndex].Quality == Quality.Low)
                    {
                        CharResultDataM.ResLow += 1;

                    }
                    else if (Question[CurrentIndex].Quality == Quality.Medium)
                    {
                        CharResultDataM.ResMedium += 1;

                    }
                    else if (Question[CurrentIndex].Quality == Quality.High)
                    {
                        CharResultDataM.ResHigh += 1;

                    }
                    else
                    {
                        CharResultDataM.ResCritical += 1;

                    }

                    ChartResult[0].Values.Add(CharResultDataM.ResLow);
                    ChartResult[1].Values.Add(CharResultDataM.ResMedium);
                    ChartResult[2].Values.Add(CharResultDataM.ResHigh);
                    ChartResult[3].Values.Add(CharResultDataM.ResCritical);
                }


                ChartSeries[0].Values.Add(SeriesData.RespYes);
                ChartSeries[1].Values.Add(SeriesData.RespNo);
                ChartSeries[2].Values.Add(SeriesData.RespDonTKnow);
                ChartSeries[3].Values.Add(SeriesData.RespNone);
            }

            ResponseCurrUpdate();
            NextQuestion();


        }
        private void NextQuestion()
        {


            if(CurrentIndex < Question.Count) CurrentIndex++;
            Percentage = (int)((double)CurrentIndex / Question.Count * 100);
            CurrentIndexQuestion = CurrentIndex + "/" + Question.Count;

            if (CurrentIndex >= Question.Count)
            {
                ResultPercent = GetResultPercentText();

                OpenIsoResultsWindowCommand.Execute(this);
                return;
            }

            QuestionCurr = GetDescription(Question[CurrentIndex]);
        }




        #endregion

        #region Response

        private string _responseCurr;

        public string ResponseCurr
        {
            get => _responseCurr;
            set => Set(ref _responseCurr,value);
        }

        private string _threatLevel;

        public string ThreatLevel
        {
            get => _threatLevel;
            set => Set(ref _threatLevel, value);

        }

        private void ResponseCurrUpdate()
        {
           if(CurrentIndex < Question.Count) {
                if (Question[CurrentIndex].Resolved == Answer.Yes)
                {
                    ThreatLevel = _localization?.Safely ?? "Safely";
                    ResponseCurr = GetTitle(Question[CurrentIndex]) + "\n" + GetRespPos(Question[CurrentIndex]);

                }
                else if (Question[CurrentIndex].Resolved == Answer.No)
                {
                    ResponseCurr = GetTitle(Question[CurrentIndex]) + "\n" + GetRespNeg(Question[CurrentIndex]);
                    ThreatLevel = GetThreatLevelText(Question[CurrentIndex].Quality);

                }
                else if (Question[CurrentIndex].Resolved == Answer.DonTKnow)
                {
                    ResponseCurr = GetTitle(Question[CurrentIndex]) + "\n" + GetRespNeg(Question[CurrentIndex]);
                    ThreatLevel = GetThreatLevelText(Question[CurrentIndex].Quality);
                }
                else
                {
                    ResponseCurr ="<<<"+ GetTitle(Question[CurrentIndex]) +">>>" + "\n" + GetRespNeg(Question[CurrentIndex]);
                    ThreatLevel = GetThreatLevelText(Question[CurrentIndex].Quality);
                }
           }
        }

        private bool IsEnglish => _localization?.IsEnglish == true;

        private string GetDescription(QuestionModel question)
        {
            return IsEnglish && EnglishQuestions.TryGetValue(question.Number, out var english)
                ? english.Description
                : question.Description;
        }

        private string GetTitle(QuestionModel question)
        {
            return IsEnglish && EnglishQuestions.TryGetValue(question.Number, out var english)
                ? english.Title
                : question.Title;
        }

        private string GetRespPos(QuestionModel question)
        {
            return IsEnglish && EnglishQuestions.TryGetValue(question.Number, out var english)
                ? english.RespPos
                : question.RespPos;
        }

        private string GetRespNeg(QuestionModel question)
        {
            return IsEnglish && EnglishQuestions.TryGetValue(question.Number, out var english)
                ? english.RespNeg
                : question.RespNeg;
        }

        private string GetResultPercentText()
        {
            string percent = ((double)SeriesData.RespYes / Question.Count * 100).ToString("0.##");
            return IsEnglish
                ? "ISO 27001 readiness: " + percent + "%"
                : "В вашей компании ISO 27001 выполняется на " + percent + "%";
        }

        private void RefreshLanguage()
        {
            int selectedRiskIndex = Items?.IndexOf(SelectedItemMiniCommand) ?? -1;

            TextStart = GetIsoOverviewText();
            TextInfoIso = GetIsoInfoText();
            UpdateLocalizedCollections();

            if (CurrentIndex < Question.Count)
            {
                QuestionCurr = GetDescription(Question[CurrentIndex]);
                ResponseCurrUpdate();
            }
            else
            {
                ResultPercent = GetResultPercentText();
            }

            if (selectedRiskIndex >= 0 && selectedRiskIndex < Items.Count)
            {
                SelectedItemMiniCommand = Items[selectedRiskIndex];
            }
            else
            {
                CurrUpdateResult();
            }
        }

        private string GetIsoOverviewText()
        {
            if (!IsEnglish)
            {
                return BaseTextInfo?.TextStart ?? string.Empty;
            }

            return "Information security, cybersecurity, and privacy protection. Information security management systems. Requirements.\r\n" +
                   "This overview is based on ISO/IEC 27001:2022 and explains the purpose of an information security management system (ISMS).\r\n\r\n" +
                   "An ISMS helps an organization protect confidentiality, integrity, and availability of information through a risk management process. It should be integrated into organizational processes, governance, information systems, and control design.\r\n\r\n" +
                   "The questionnaire in this program is a practical self-assessment tool. It is not a certification audit and does not replace professional legal or compliance review.";
        }

        private string GetIsoInfoText()
        {
            if (!IsEnglish)
            {
                return (BaseTextInfo?.TextInf ?? string.Empty) +
                       "\r\n\r\nМодель Risk Analysis, используемая здесь:\r\n" +
                       "SLE - ожидаемые денежные потери от одного инцидента до поправки на exposure factor.\r\n" +
                       "EF - доля воздействия от 0 до 1. Например, 0.25 означает 25% от SLE.\r\n" +
                       "ARO - ожидаемая частота событий в год. Например, 1 раз в 2 года = 0.5/yr; 3 раза в 6 месяцев = 6/yr.\r\n" +
                       "ALE = SLE * EF * ARO. Это оценка ожидаемых годовых потерь, а не точный ущерб и не юридическое подтверждение соответствия.";
            }

            return "Key ISO 27001 areas covered by this tool:\r\n\r\n" +
                   "5.2 Policy: information security policy should be approved, communicated, maintained, and aligned with organizational objectives.\r\n\r\n" +
                   "5.3 Roles and responsibilities: security roles, responsibilities, and authorities should be assigned and communicated.\r\n\r\n" +
                   "6 Risk planning: information security risks should be identified, assessed, treated, and reviewed.\r\n\r\n" +
                   "7 Support: competence, awareness, communication, and documented information should be managed.\r\n\r\n" +
                   "8 Operation: risk treatment plans, operational controls, supplier controls, access control, logging, change control, backups, and incident handling should be implemented and monitored.\r\n\r\n" +
                   "9 Performance evaluation: monitoring, measurement, internal audit, and management review should support improvement.\r\n\r\n" +
                   "10 Improvement: nonconformities should lead to corrective actions and continual improvement.\r\n\r\n" +
                   "Risk Analysis model used here:\r\n" +
                   "SLE is the expected monetary loss from one incident before the exposure factor adjustment.\r\n" +
                   "EF is the exposure factor as a decimal fraction from 0 to 1. Example: 0.25 means 25% of the SLE is expected to be lost.\r\n" +
                   "ARO is the annualized rate of occurrence. Example: 1 time per 2 years = 0.5/yr; 3 times per 6 months = 6/yr.\r\n" +
                   "ALE = SLE * EF * ARO. It estimates annualized loss exposure, not exact damage or legal compliance.";
        }

        private void UpdateLocalizedCollections()
        {
            if (ChartSeries?.Count >= 4)
            {
                if (ChartSeries[0] is PieSeries yesSeries) yesSeries.Title = _localization?.Yes ?? "Yes";
                if (ChartSeries[1] is PieSeries noSeries) noSeries.Title = _localization?.No ?? "No";
                if (ChartSeries[2] is PieSeries unknownSeries) unknownSeries.Title = _localization?.DoNotKnow ?? "Do not know";
                if (ChartSeries[3] is PieSeries pendingSeries) pendingSeries.Title = IsEnglish ? "Pending" : "Ждет ответа";
            }

            if (ChartResult?.Count >= 4)
            {
                if (ChartResult[0] is PieSeries lowSeries) lowSeries.Title = _localization?.Low ?? "Low";
                if (ChartResult[1] is PieSeries mediumSeries) mediumSeries.Title = _localization?.Medium ?? "Medium";
                if (ChartResult[2] is PieSeries highSeries) highSeries.Title = _localization?.High ?? "High";
                if (ChartResult[3] is PieSeries criticalSeries) criticalSeries.Title = _localization?.Critical ?? "Critical";
            }

            Items = new ObservableCollection<string>
            {
                _localization?.Low ?? "Low",
                _localization?.Medium ?? "Medium",
                _localization?.High ?? "High",
                _localization?.Critical ?? "Critical"
            };
        }

        private string GetThreatLevelText(Quality quality)
        {
            return quality switch
            {
                Quality.Low => _localization?.Low ?? "Low",
                Quality.Medium => _localization?.Medium ?? "Medium",
                Quality.High => _localization?.High ?? "High",
                Quality.Critical => _localization?.Critical ?? "Critical",
                _ => _localization?.None ?? "None"
            };
        }




        public bool IsEndQuestion()
        {
            if (CurrentIndex >= Question.Count)
            {
                return true;
            }

            return false;
        }


        private double _verticalScrollBarValue;
        public double VerticalScrollBarValue
        {
            get { return _verticalScrollBarValue; }
            set { Set(ref _verticalScrollBarValue, value); }
        }
        private void TextBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            VerticalScrollBarValue = e.VerticalOffset;
        }




        private ObservableCollection<string> _items;
        public ObservableCollection<string> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }


        private string _selectedItemMiniCommand;
        public string SelectedItemMiniCommand
        {
            get => _selectedItemMiniCommand;
            set
            {
                if (_selectedItemMiniCommand == value) return;
                _selectedItemMiniCommand = value;
                OnPropertyChanged(nameof(SelectedItemMiniCommand));
                CurrUpdateResult();
            }
        }

        private string _textResult;

        public string TextResult
        {
            get=> _textResult;
            set=> Set(ref _textResult, value);
        }

        private void CurrUpdateResult()
        {
            if (SelectedItemMiniCommand == Items[0])
            {
                StringBuilder formattedText = new StringBuilder();

                foreach (var questionModel in Question)
                {
                    if (questionModel.Resolved == Answer.No || questionModel.Resolved == Answer.None)
                    {
                        if (questionModel.Quality == Quality.Low)
                        {
                            formattedText.Append("!!!");
                            formattedText.Append(GetTitle(questionModel));
                            formattedText.Append("!!!\n");
                            formattedText.Append(GetRespNeg(questionModel));
                            formattedText.Append("\n\n\n");
                        }
                    }
                }

                TextResult = formattedText.ToString();

            }
            else if (SelectedItemMiniCommand == Items[1])
            {
                StringBuilder formattedText = new StringBuilder();

                foreach (var questionModel in Question)
                {
                    if (questionModel.Resolved == Answer.No || questionModel.Resolved == Answer.None)
                    {
                        if (questionModel.Quality == Quality.Medium)
                        {
                            formattedText.Append("!!!");
                            formattedText.Append(GetTitle(questionModel));
                            formattedText.Append("!!!\n");
                            formattedText.Append(GetRespNeg(questionModel));
                            formattedText.Append("\n\n\n");
                        }
                    }
                }

                TextResult = formattedText.ToString();

            }
            else if (SelectedItemMiniCommand == Items[2])
            {
                StringBuilder formattedText = new StringBuilder();

                foreach (var questionModel in Question)
                {
                    if (questionModel.Resolved == Answer.No || questionModel.Resolved == Answer.None)
                    {
                        if (questionModel.Quality == Quality.High)
                        {
                            formattedText.Append("!!!");
                            formattedText.Append(GetTitle(questionModel));
                            formattedText.Append("!!!\n");
                            formattedText.Append(GetRespNeg(questionModel));
                            formattedText.Append("\n\n\n");
                        }
                    }
                }

                TextResult = formattedText.ToString();

            }
            else if (SelectedItemMiniCommand == Items[3])
            {
                StringBuilder formattedText = new StringBuilder();

                foreach (var questionModel in Question)
                {
                    if (questionModel.Resolved == Answer.No || questionModel.Resolved == Answer.None)
                    {
                        if (questionModel.Quality == Quality.Critical)
                        {
                            formattedText.Append("!!!");
                            formattedText.Append(GetTitle(questionModel));
                            formattedText.Append("!!!\n");
                            formattedText.Append(GetRespNeg(questionModel));
                            formattedText.Append("\n\n\n");
                        }
                    }
                }

                TextResult = formattedText.ToString();
            }
        }   




        #endregion




    }
}
