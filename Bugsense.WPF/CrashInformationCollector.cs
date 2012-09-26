using System;
using System.Reflection;
using System.Text;

namespace Bugsense.WPF
{
    internal class CrashInformationCollector
    {
        private readonly string _version;

        public CrashInformationCollector(string version)
        {
            _version = version;
        }
        
        public BugSenseRequest CreateCrashReport(Exception exception)
        {
            var entryAssemblyName = Assembly.GetEntryAssembly().GetName();
            var operatingSystem = Environment.OSVersion;

            var fullStacktrace = GetStackTrace(exception);

            return new BugSenseRequest(
                new BugSenseEx
                    {
                        ExceptionType = exception.GetType().ToString(),
                        Message = exception.Message,
                        DateOccured = DateTime.UtcNow,
                        StackTrace = fullStacktrace
                    },
                new AppEnvironment
                    {
                        AppName = entryAssemblyName.Name,
                        AppVersion = _version ?? entryAssemblyName.Version.ToString(4),
                        OsVersion = operatingSystem.Version.ToString(4)
                    }
                );
        }

        private static string GetStackTrace(Exception exception)
        {
            var sb = new StringBuilder();
            var ex = exception;
            sb.AppendLine(string.IsNullOrEmpty(ex.StackTrace) ? "not available" : ex.StackTrace);
            ex = ex.InnerException;
            while (ex != null)
            {
                sb.AppendLine(string.Format("Caused by: {0}: {1}", ex.GetType().Name, ex.Message));
                sb.AppendLine(string.IsNullOrEmpty(ex.StackTrace) ? "not available" : ex.StackTrace);
                ex = ex.InnerException;
            }
            return sb.ToString();
        }
    }
}