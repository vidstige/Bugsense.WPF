using System;
using System.Windows.Input;

namespace SampleApplication
{
    public class CrashCommand: ICommand
    {
        public void Execute(object parameter)
        {
            throw new NotImplementedException("Thrown intentionally");
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
