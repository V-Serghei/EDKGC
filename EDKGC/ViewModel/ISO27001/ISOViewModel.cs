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
using EDKGC.Infrastructure.Command.BasicCommands;
using EDKGC.Models.ISO27001;
using EDKGC.ViewModel.SatelliteWindows;
using GrapeCity.DataVisualization.TypeScript;
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

        public string TextStart { get; }
        public string TextInfoIso { get; }

        public TextInfo BaseTextInfo { get; }

        public string ResultPercent { get; set; }

        #endregion

        public ISOViewModel()
        {


            Question = new List<QuestionModel>();
            string basePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string jsonPath = Path.Combine(basePath, @"Data\Seed\ISOQuestion.json");
            string json = File.ReadAllText(jsonPath);
            Question = JsonConvert.DeserializeObject<List<QuestionModel>>(json);
            OpenIsoResultsWindowCommand = new OpenIsoResultsWindowCommand(this);
            BaseTextInfo = new TextInfo();
            TextStart = BaseTextInfo.TextStart;
            TextInfoIso = BaseTextInfo.TextInf;

            ChartSeries = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Да",
                    Values = new ChartValues<int>(),
                    Fill = Brushes.Green
                },
                new PieSeries
                {
                    Title = "Нет",
                    Values = new ChartValues<int>(),
                    Fill = Brushes.Red
                },
                new PieSeries
                {
                    Title = "Не знаю",
                    Values = new ChartValues<int>(),
                    Fill = Brushes.Yellow
                },
                new PieSeries
                {
                    Title = "Ждет ответа",
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



            RespYesCommand = new RelayCommand(ResolveYesCommand);

            RespNoCommand = new RelayCommand(ResolveNoCommand);

            RespDonTKnowCommand = new RelayCommand(ResolveDonTKnowCommand);

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


            Items = new ObservableCollection<string>() {  "Low",
                "Medium",
                "High",
                "Critical" };

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
                ResultPercent = "В вашей компании ISO 27001 выполняется на " +
                                ((double)SeriesData.RespYes / Question.Count * 100).ToString("0.##") + "%";

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
                    ThreatLevel = "Safely";
                    ResponseCurr = Question[CurrentIndex].Title + "\n" + Question[CurrentIndex].RespPos;

                }
                else if (Question[CurrentIndex].Resolved == Answer.No)
                {
                    ResponseCurr = Question[CurrentIndex].Title + "\n" + Question[CurrentIndex].RespNeg;
                    if (Question[CurrentIndex].Quality == Quality.High)
                    {
                        ThreatLevel = "High";
                    }
                    else if (Question[CurrentIndex].Quality == Quality.Low)
                    {
                        ThreatLevel = "Low";
                    }
                    else if (Question[CurrentIndex].Quality == Quality.Critical)
                    {
                        ThreatLevel = "Critical";
                    }
                    else if (Question[CurrentIndex].Quality == Quality.Medium)
                    {
                        ThreatLevel = "Medium";
                    }
                    else
                    {
                        ThreatLevel = "None";
                    }

                }
                else if (Question[CurrentIndex].Resolved == Answer.DonTKnow)
                {
                    ResponseCurr = Question[CurrentIndex].Title + "\n" + Question[CurrentIndex].RespNeg;
                    if (Question[CurrentIndex].Quality == Quality.High)
                    {
                        ThreatLevel = "High";
                    }
                    else if (Question[CurrentIndex].Quality == Quality.Low)
                    {
                        ThreatLevel = "Low";
                    }
                    else if (Question[CurrentIndex].Quality == Quality.Critical)
                    {
                        ThreatLevel = "Critical";
                    }
                    else if (Question[CurrentIndex].Quality == Quality.Medium)
                    {
                        ThreatLevel = "Medium";
                    }
                    else
                    {
                        ThreatLevel = "None";
                    }
                }
                else
                {
                    ResponseCurr = Question[CurrentIndex].Title + "\n" + Question[CurrentIndex].RespNeg;
                    if (Question[CurrentIndex].Quality == Quality.High)
                    {
                        ThreatLevel = "High";
                    }
                    else if (Question[CurrentIndex].Quality == Quality.Low)
                    {
                        ThreatLevel = "Low";
                    }
                    else if (Question[CurrentIndex].Quality == Quality.Critical)
                    {
                        ThreatLevel = "Critical";
                    }
                    else if (Question[CurrentIndex].Quality == Quality.Medium)
                    {
                        ThreatLevel = "Medium";
                    }
                    else
                    {
                        ThreatLevel = "None";
                    }
                }
           }
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

        //  private void CurrUpdateResult()
        //{
        //    if (SelectedItemMiniCommand == Items[0])
        //    {
        //        foreach (var questionModel in Question)
        //        {
        //            if (questionModel.Resolved == Answer.No || questionModel.Resolved == Answer.None)
        //            {
        //                if (questionModel.Quality == Quality.Low)
        //                {
        //                    TextResult = "!!!" + questionModel.Title + "!!!\n" + questionModel.RespNeg + "\n";
        //                }
        //            }
        //        }

        //    }
        //    else if (SelectedItemMiniCommand == Items[1])
        //    {
        //        foreach (var questionModel in Question)
        //        {
        //            if (questionModel.Resolved == Answer.No || questionModel.Resolved == Answer.None)
        //            {
        //                if (questionModel.Quality == Quality.Medium)
        //                {
        //                    TextResult = "!!!" + questionModel.Title + "!!!\n" + questionModel.RespNeg + "\n";
        //                }
        //            }
        //        }

        //    }
        //    else if (SelectedItemMiniCommand == Items[2])
        //    {
        //        foreach (var questionModel in Question)
        //        {
        //            if (questionModel.Resolved == Answer.No || questionModel.Resolved == Answer.None)
        //            {
        //                if (questionModel.Quality == Quality.High)
        //                {
        //                    TextResult = "!!!" + questionModel.Title + "!!!\n" + questionModel.RespNeg + "\n";
        //                }
        //            }
        //        }

        //    }
        //    else if (SelectedItemMiniCommand == Items[3])
        //    {
        //        foreach (var questionModel in Question)
        //        {
        //            if (questionModel.Resolved == Answer.No || questionModel.Resolved == Answer.None)
        //            {
        //                if (questionModel.Quality == Quality.Critical)
        //                {
        //                    TextResult = "!!!" + questionModel.Title + "!!!\n" + questionModel.RespNeg +"\n";
        //                }
        //            }
        //        }
        //    }
        //}

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
                            formattedText.Append(questionModel.Title);
                            formattedText.Append("!!!\n");
                            formattedText.Append(questionModel.RespNeg);
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
                            formattedText.Append(questionModel.Title);
                            formattedText.Append("!!!\n");
                            formattedText.Append(questionModel.RespNeg);
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
                            formattedText.Append(questionModel.Title);
                            formattedText.Append("!!!\n");
                            formattedText.Append(questionModel.RespNeg);
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
                            formattedText.Append(questionModel.Title);
                            formattedText.Append("!!!\n");
                            formattedText.Append(questionModel.RespNeg);
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