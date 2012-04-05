using System.Diagnostics;
using System;

namespace Bugsense.WPF
{
    /// <summary>
    /// This class is used to hook up the AppDomain.UnhandledException handler to a Bugsense error sender that
    /// immediately sends a crash report to the bugsense servers.
    /// </summary>
    public static class BugSense
    {
        private static ErrorSender errorSender;
        private static CrashInformationCollector informationCollector;

        /// <summary>
        /// Hooks up bugsense error sender to the unhandled exception handler. This will cause crashes to be sent
        /// to bugsense when they occur.
        /// </summary>
        /// <param name="apiKey">This is the API key for bugsense. You need to get *your own* API key from http://bugsense.com/</param>
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
