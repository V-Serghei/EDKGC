using System.Windows;
using EDKGC.Infrastructure.Command.Base;

namespace EDKGC.Infrastructure.Command.BasicCommands
{
    internal class CloseAppCommand: CommandInf
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => Application.Current.Shutdown();
    }
}
