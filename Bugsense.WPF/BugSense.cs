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

            AttachHandler();
        }

        /// <summary>
        /// Attaches the eventhandler from AppDomain.CurrentDomain.UnhandledException. Done Automatically by the BugSense.Init method.
        /// </summary>
        public static void AttachHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
        }
        /// <summary>
        /// Detaches the eventhandler from AppDomain.CurrentDomain.UnhandledException. Useful if you want to send exceptions your self.
        /// </summary>
        public static void DetachHandler()
        {
            AppDomain.CurrentDomain.UnhandledException -= UnhandledException;
        }

        /// <summary>
        /// Sends an exception to bugsense as if a crash occured. Useful for handling the UnhandledException manually.
        /// </summary>
        /// <param name="exception">The exception to send to BugSense</param>
        /// <exception cref="InvalidOperationException">Thrown if the Bugsense.Init method is not called before this method</exception>
        public static void SendException(Exception exception)
        {
            if (errorSender == null) throw new InvalidOperationException("BugSense.Init must be called before this method may be called");
            errorSender.SendOrStore(informationCollector.CreateCrashReport(exception));
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!Debugger.IsAttached)
            {
                SendException((Exception)e.ExceptionObject);
            }
        }
    }
}
