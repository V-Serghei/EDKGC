using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using EDKGC.Enams;
using EDKGC.Models.ISO27001;
using EDKGC.ViewModel.SatelliteWindows;
using GrapeCity.DataVisualization.TypeScript;
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

        public PieChartData SeriesData { get; set; }


        #endregion

        public ISOViewModel()
        {
            Question = new List<QuestionModel>();
            string basePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string jsonPath = Path.Combine(basePath, @"Data\Seed\ISOQuestion.json");
            string json = File.ReadAllText(jsonPath);
            Question = JsonConvert.DeserializeObject<List<QuestionModel>>(json);


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



            RespYesCommand = new RelayCommand(ResolveYesCommand);

            RespNoCommand = new RelayCommand(ResolveNoCommand);

            RespDonTKnowCommand = new RelayCommand(ResolveDonTKnowCommand);

            CurrentQuestion = Question[CurrentIndex];

            _question = CurrentQuestion.Description;

            SeriesData = new PieChartData
            {
                RespYes = 0,
                RespNo = 0,
                RespDonTKnow = 0,
                RespNone = Question.Count
            };
            CurrentIndexQuestion = CurrentIndex + "/" + Question.Count;

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


            CurrentIndex++;
            Percentage = (int)((double)CurrentIndex / Question.Count * 100);
            CurrentIndexQuestion = CurrentIndex + "/" + Question.Count;

            if (CurrentIndex >= Question.Count)
            {


                //here we add logic to the conclusion of the number of questions
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





        #endregion


#

    }
}