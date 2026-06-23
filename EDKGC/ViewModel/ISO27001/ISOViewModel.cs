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
using GalaSoft.MvvmLight.Ioc;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
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

        public string ResultPercent { get; set; }

        #endregion

        private List<QuestionModel> LoadQuestions()
        {
            string fileName = _localization?.IsEnglish == true
                ? "ISOQuestion.en.json"
                : "ISOQuestion.ru.json";
            string jsonPath = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", fileName);

            if (!File.Exists(jsonPath))
            {
                jsonPath = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "ISOQuestion.json");
            }

            string json = File.ReadAllText(jsonPath);
            return JsonConvert.DeserializeObject<List<QuestionModel>>(json) ?? new List<QuestionModel>();
        }

        public ISOViewModel()
        {


            OpenIsoResultsWindowCommand = new OpenIsoResultsWindowCommand(this);
            try
            {
                _localization = SimpleIoc.Default.GetInstance<LocalizationViewModel>();
            }
            catch
            {
                _localization = null;
            }

            Question = LoadQuestions();

            if (_localization != null)
            {
                _localization.PropertyChanged += (_, _) => RefreshLanguage();
            }
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
                    Title = _localization?.Pending ?? "Pending",
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

            _question = CurrentQuestion.Description;

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

            QuestionCurr = Question[CurrentIndex].Description;
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

        private string GetTitle(QuestionModel question)
        {
            return question.Title;
        }

        private string GetRespPos(QuestionModel question)
        {
            return question.RespPos;
        }

        private string GetRespNeg(QuestionModel question)
        {
            return question.RespNeg;
        }

        private string GetResultPercentText()
        {
            string percent = ((double)SeriesData.RespYes / Question.Count * 100).ToString("0.##");
            string format = _localization?.ResultPercentFormat ?? "ISO 27001 readiness: {0}%";
            return string.Format(format, percent);
        }

        private void RefreshLanguage()
        {
            var resolvedByNumber = new Dictionary<int, Answer>();
            foreach (var question in Question)
            {
                resolvedByNumber[question.Number] = question.Resolved;
            }

            int selectedRiskIndex = Items?.IndexOf(SelectedItemMiniCommand) ?? -1;
            Question = LoadQuestions();
            foreach (var question in Question)
            {
                if (resolvedByNumber.TryGetValue(question.Number, out var resolved))
                {
                    question.Resolved = resolved;
                }
            }

            TextStart = GetIsoOverviewText();
            TextInfoIso = GetIsoInfoText();
            UpdateLocalizedCollections();

            if (CurrentIndex < Question.Count)
            {
                CurrentQuestion = Question[CurrentIndex];
                QuestionCurr = CurrentQuestion.Description;
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
            return _localization?.IsoOverviewText ?? string.Empty;
        }

        private string GetIsoInfoText()
        {
            return _localization?.IsoInfoText ?? string.Empty;
        }

        private void UpdateLocalizedCollections()
        {
            if (ChartSeries?.Count >= 4)
            {
                if (ChartSeries[0] is PieSeries yesSeries) yesSeries.Title = _localization?.Yes ?? "Yes";
                if (ChartSeries[1] is PieSeries noSeries) noSeries.Title = _localization?.No ?? "No";
                if (ChartSeries[2] is PieSeries unknownSeries) unknownSeries.Title = _localization?.DoNotKnow ?? "Do not know";
                if (ChartSeries[3] is PieSeries pendingSeries) pendingSeries.Title = _localization?.Pending ?? "Pending";
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
