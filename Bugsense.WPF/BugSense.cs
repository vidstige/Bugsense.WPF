using System.Windows;

namespace Bugsense.WPF
{
    public static class BugSense
    {
        public static void Init(Application app, string apiKey)
        {
            new ExceptionWatcher(app, new ErrorSender(apiKey), new CrashInformationCollector());
        }
    }
}
