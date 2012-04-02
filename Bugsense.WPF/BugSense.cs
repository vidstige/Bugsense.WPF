using System.Diagnostics;
using System;

namespace Bugsense.WPF
{
    public static class BugSense
    {
        private static ErrorSender errorSender;
        private static CrashInformationCollector informationCollector;

        public static void Init(string apiKey)
        {
            errorSender = new ErrorSender(apiKey);
            informationCollector = new CrashInformationCollector();

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!Debugger.IsAttached)
            {
                SendException((Exception) e.ExceptionObject);
            }
            Environment.Exit(-1);
        }

        private static void SendException(Exception exception)
        {
            errorSender.Send(informationCollector.CreateCrashReport(exception));
        }
    }
}
