using System;
using System.Text;

namespace Bugsense.WPF
{
    internal class CrashInformationCollector
    {
        private readonly IAssemblyRepository _assemblyRepository;
        private readonly string _version;

        public CrashInformationCollector(IAssemblyRepository assemblyRepository, string version = null)
        {
            _assemblyRepository = assemblyRepository;
            _version = version;
        }
        
        public BugSenseRequest CreateCrashReport(Exception exception)
        {
            var entryAssemblyName = _assemblyRepository.GetEntryAssembly().GetName();
            var operatingSystem = GetOSName(Environment.OSVersion);

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
                        OsVersion = operatingSystem
                    }
                );
        }

        private static string GetOSName(OperatingSystem os)
        {
            Version version = os.Version;

            // perform simple detection of OS
            // TODO: more sophisticated detection; Windows 7 and Server 2008 have the same major/minor version number 
            string release;
            if (version.Major == 5 && version.Minor == 1)
                release = "XP";
            else if (version.Major == 5 && version.Minor == 2)
                release = "Server 2003";
            else if (version.Major == 6 && version.Minor == 0)
                release = "Vista";
            else if (version.Major == 6 && version.Minor == 1)
                release = "7";
            else if (version.Major == 6 && version.Minor == 2)
                release = "8";
            else if (version.Major == 6 && version.Minor == 3)
                release = "8.1";
            else
                release = version.ToString();

            string servicePack = string.IsNullOrEmpty(os.ServicePack) ? "" : (" " + os.ServicePack.Replace("Service Pack ", "SP"));

            return release + servicePack;
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