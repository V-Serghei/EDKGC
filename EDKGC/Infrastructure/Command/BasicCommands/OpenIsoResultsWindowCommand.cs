using EDKGC.ViewModel.ISO27001;
using EDKGC.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EDKGC.Infrastructure.Command.BasicCommands
{
    public class OpenIsoResultsWindowCommand : ICommand
    {
        private readonly ISOViewModel _viewModel;

        public OpenIsoResultsWindowCommand(ISOViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            // Возвращаем true, если выполнена условие IsEndQuestion
            //return _viewModel.IsEndQuestion();
            return true;

        }

        public void Execute(object parameter)
        {
            // Создаем и открываем новое окно только если условие IsEndQuestion выполнено
            if (_viewModel.IsEndQuestion())
            {
                var window = new IsoResultsWidow();
                window.Show();
            }
            else
            {
                MessageBox.Show("Для открытия окна необходимо ответить на все вопросы.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
