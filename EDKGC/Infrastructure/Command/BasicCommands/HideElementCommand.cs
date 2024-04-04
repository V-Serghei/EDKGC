using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using EDKGC.Infrastructure.Command.Base;

namespace EDKGC.Infrastructure.Command.BasicCommands
{
    internal class HideElementCommand : CommandInf
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            if (parameter is Button button)
            {
                button.Visibility = Visibility.Collapsed;
            }
            else if (parameter is TextBlock textBlock)
            {
                textBlock.Visibility = Visibility.Collapsed;
            }
            else if (parameter is Rectangle rect)
            {
                rect.Visibility = Visibility.Collapsed;
            }
        }
    }
}
