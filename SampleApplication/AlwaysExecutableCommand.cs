using System;
using System.Windows.Input;

namespace SampleApplication
{
    public abstract class AlwaysExecutableCommand: ICommand
    {
        public abstract void Execute(object parameter);

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}