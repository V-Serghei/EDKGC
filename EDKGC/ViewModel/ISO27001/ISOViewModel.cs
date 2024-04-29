using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using EDKGC.Enams;
using EDKGC.Models.ISO27001;
using EDKGC.ViewModel.SatelliteWindows;
using GrapeCity.DataVisualization.TypeScript;
using LiveCharts;
using LiveCharts.Wpf;
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
            Question.push(new QuestionModel
            {
                Description = "1)sdjfsdfsdfsfsdfsdfsdfsfsfsdfsdfsfs",
                Number = 1,
                Quality = Quality.High,
                Resolved = Answer.None,
                Title = "Prom"

            });
            Question.push(new QuestionModel
            {
                Description = "2)sdjfsdfsdfsfsdfsdfsdfsfsfsdfsdfsfs",
                Number = 2,
                Quality = Quality.High,
                Resolved = Answer.None,
                Title = "Prom"

            });
            Question.push(new QuestionModel
            {
                Description = "3)sdjfsdfsdfsfsdfsdfsdfsfsfsdfsdfsfs",
                Number = 3,
                Quality = Quality.High,
                Resolved = Answer.None,
                Title = "Prom"

            });
            Question.push(new QuestionModel
            {
                Description = "4)sdjfsdfsdfsfsdfsdfsdfsfsfsdfsdfsfs",
                Number = 4,
                Quality = Quality.High,
                Resolved = Answer.None,
                Title = "Prom"

            });

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
                    Title = "Не определён",
                    Values = new ChartValues<int>{4},
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

        }

        

        #region questions

        private string _question;
        private ICommand _respYesCommand;
        private ICommand _respNoCommand;
        private ICommand _respDonTKnowCommand;

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

            NextQuestion();


        }
        private void NextQuestion()
        {
            CurrentIndex++;

            if (CurrentIndex >= Question.Count)
            {


                //here we add logic to the conclusion of the number of questions
                return;
            }

            QuestionCurr = Question[CurrentIndex].Description;
        }




        #endregion

    }
}