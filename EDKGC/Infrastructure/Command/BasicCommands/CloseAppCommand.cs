using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EDKGC.Infrastructure.Command.Base;

namespace EDKGC.Infrastructure.Command.BasicCommands
{
    internal class CloseAppCommand: CommandInf
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => Application.Current.Shutdown();
    }
}
