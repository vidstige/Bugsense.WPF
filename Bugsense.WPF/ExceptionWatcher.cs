using System;
using System.Windows;
using System.Windows.Threading;

namespace Bugsense.WPF
{
    class ExceptionWatcher
    {
        private readonly ErrorSender errorSender;
        private readonly CrashInformationCollector inforCollector;

        public ExceptionWatcher(Application app, ErrorSender errorSender, CrashInformationCollector inforCollector)
        {
            this.errorSender = errorSender;
            this.inforCollector = inforCollector;
            app.DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            errorSender.Send(inforCollector.CreateCrashReport(e.Exception));
        }
    }
}