using System;
using System.Threading;

namespace SampleApplication
{
    public static class CrashHelper
    {
        public static void Crash()
        {
            throw new ArgumentException("Thrown intentionally");
        }
    }

    public class CrashCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            try
            {
                try
                {
                    ThrowApplicationException("Inner");
                }
                catch (ApplicationException ex)
                {
                    ThrowApplicationException("Middle", ex);
                }
            }
            catch (ApplicationException ex)
            {
                ThrowApplicationException("Outer", ex);
            }
        }

        private void ThrowApplicationException(string message, Exception innerException)
        {
            FillTheStacktrace(message, innerException);
        }

        private static void FillTheStacktrace(string message, Exception innerException)
        {
            throw new ApplicationException(message, innerException);
        }

        private static void ThrowApplicationException(string message)
        {
            ThisMessageJustFillsTheInnerStacktrace(message);
        }

        private static void ThisMessageJustFillsTheInnerStacktrace(string message)
        {
            throw new ApplicationException(message);
        }
    }

    public class BackgroundCrashCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            ThreadPool.QueueUserWorkItem(Crash);
        }

        private void Crash(object state)
        {
            CrashHelper.Crash();
        }
    }
}
