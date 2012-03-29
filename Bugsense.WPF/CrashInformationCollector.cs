using System;

namespace Bugsense.WPF
{
    internal class CrashInformationCollector
    {
        public ErrorReport CreateCrashReport(Exception exception)
        {
            var occuredAt = DateTime.Now;

            return new ErrorReport();
        }
    }
}