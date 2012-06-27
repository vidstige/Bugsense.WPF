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
        private const string bugsenseApiUrl = "http://bugsense.appspot.com/api/errors";
        
        /// <summary>
        /// Hooks up bugsense error sender to the unhandled exception handler. This will cause crashes to be sent
        /// to bugsense when they occur. This overload is used when customizing the crash report destination. For normal
        /// use - Use the other Init method.
        /// </summary>
        /// <param name="apiKey">This is the API key for bugsense. You need to get *your own* API key from http://bugsense.com/ </param>
        /// <param name="version">The version of this application to send in case of a crash. The default value is zero, in this case the version of the entry assebmly will be used. See Assembly.GetEntryAssembly()</param>
        /// <param name="apiUrl">The Url to send the crashes to, only use this if you need to customize the destination</param>
        public static void Init(string apiKey, string version = null, string apiUrl = bugsenseApiUrl)
        {
            errorSender = new ErrorSender(apiKey, apiUrl);
            informationCollector = new CrashInformationCollector(version);

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
            errorSender.SendOrStore(informationCollector.CreateCrashReport(exception));
        }
    }
}
