using System.Windows;
using System.Windows.Threading;

namespace Bugsense.WPF
{
    public static class BugSense
    {
        private static ErrorSender errorSender;
        private static CrashInformationCollector informationCollector;

        public static void Init(Application app, string apiKey)
        {
            errorSender = new ErrorSender(apiKey);
            informationCollector = new CrashInformationCollector();

            app.DispatcherUnhandledException += UnhandledException;
        }

        private static void UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            errorSender.Send(informationCollector.CreateCrashReport(e.Exception));
        }
    }
}
