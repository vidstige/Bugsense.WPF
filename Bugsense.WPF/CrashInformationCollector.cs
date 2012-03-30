using System;
using System.Reflection;
using System.Text;

namespace Bugsense.WPF
{
    internal class CrashInformationCollector
    {
        public BugSenseRequest CreateCrashReport(Exception exception)
        {
            // TODO: Handle inner exceptions?
            var entryAssemblyName = Assembly.GetEntryAssembly().GetName();
            var operatingSystem = Environment.OSVersion;

            var fullStacktrace = GetStackTrace(exception);

            return new BugSenseRequest(
                new BugSenseEx
                    {
                        ExceptionType = exception.GetType().ToString(),
                        Message = exception.Message,
                        DateOccured = DateTime.Now,
                        StackTrace = fullStacktrace
                    },
                new AppEnvironment
                    {
                        AppName = entryAssemblyName.Name,
                        AppVersion = entryAssemblyName.Version.ToString(4),
                        OsVersion = operatingSystem.Version.ToString(4)
                    }
                );
        }

        // TODO: This appends the stacktraces in the reverse order compared to Exception.ToString()...? Use the same order?
        private static string GetStackTrace(Exception exception)
        {
            var sb = new StringBuilder();
            for (var ex = exception; ex != null; ex = ex.InnerException)
            {
                sb.AppendLine(string.IsNullOrEmpty(ex.StackTrace) ? "not available" : ex.StackTrace);
            }
            return sb.ToString().Trim();
        }
    }
}