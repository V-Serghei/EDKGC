using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EDKGC.Enams;
using EDKGC.Models.ISO27001;
using EDKGC.ViewModel.SatelliteWindows;
using GrapeCity.DataVisualization.TypeScript;
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

            RespYesCommand = new RelayCommand(ResolveQuestionCommand);

            RespNoCommand = new RelayCommand(ResolveQuestionCommand);

            RespDonTKnowCommand = new RelayCommand(ResolveQuestionCommand);

            CurrentQuestion = Question[CurrentIndex];

            _question = CurrentQuestion.Description;

        }

        

        #region questions

        private string _question = "Welcome";
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


        private void ResolveQuestionCommand(object obj)
        {
            // Установка ответа для текущего вопроса
            CurrentQuestion.Resolved = CurrAnswer;

            // Переход к следующему вопросу
            NextQuestion();


        }
        private void NextQuestion()
        {
            // Переход к следующему вопросу
            CurrentIndex++;

            // Если все вопросы пройдены, можете предпринять нужные действия (например, вывести результаты)
            if (CurrentIndex >= Question.Count)
            {
                // Действия при завершении викторины
                return;
            }

            // Обновление текстового поля с новым вопросом
            QuestionCurr = Question[CurrentIndex].Description;
        }




        #endregion

    }
}