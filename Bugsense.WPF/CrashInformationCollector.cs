using System;
using System.Reflection;

namespace Bugsense.WPF
{
    internal class CrashInformationCollector
    {
        public BugSenseRequest CreateCrashReport(Exception exception)
        {
            // TODO: Handle inner exceptions?
            var entryAssemblyName = Assembly.GetEntryAssembly().GetName();

            return new BugSenseRequest(
                new BugSenseEx
                    {
                        ExceptionType = exception.GetType().ToString(),
                        Message = exception.Message,
                        DateOccured = DateTime.Now,
                        StackTrace = exception.StackTrace
                    },
                new AppEnvironment
                    {
                        AppName = entryAssemblyName.Name,
                        AppVersion = entryAssemblyName.Version.ToString(4),
                        OsVersion = "7.0.7389"
                    }
                );
        }
    }
}