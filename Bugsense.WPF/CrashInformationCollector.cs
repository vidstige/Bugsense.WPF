using System;

namespace Bugsense.WPF
{
    internal class CrashInformationCollector
    {
        public BugSenseRequest CreateCrashReport(Exception exception)
        {
            // TODO: Fetch name/versions from this assembly / executing assembly.
            // TODO: Handle inner exceptions?
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
                        AppName = "Rebtel Phone",
                        AppVersion = "1.1.0.0",
                        OsVersion = "7.0.7389"
                    },
                new BugSenseClient()
                    {
                        Name = "bugsense-wpf",
                        Version = "0.0.0.2"
                    });
            
        }

          //application_environment = new
          //                     {
          //                          appname = "Rebtel Phone",
          //                          appver = "1.1.0.0",
          //                          gps_on = "unvailable", 
          //                          osver = "7.0.7389", 
          //                          phone = "Windows 7",
          //                          //screen_height = 800,
          //                          //screen_width = 480,
          //                          //screen_orientation = "unavailable",
          //                          wifi_on = "None"
          //                      },
          //                      client = new {
          //                          name = "bugsense-wp7", 
          //                          version = "bugsense-version-0.6"
          //                      }, 
          //                      exception = new {
          //                          backtrace = "at BugSenseTestApp.MainPage.Button_Click(Object sender, RoutedEventArgs e)\r\n   at System.Windows.Controls.Primitives.ButtonBase.OnClick()\r\n   at System.Windows.Controls.Button.OnClick()\r\n   at System.Windows.Controls.Primitives.ButtonBase.OnMouseLeftButtonUp(MouseButtonEventArgs e)\r\n   at System.Windows.Controls.Control.OnMouseLeftButtonUp(Control ctrl, EventArgs e)\r\n   at MS.Internal.JoltHelper.FireEvent(IntPtr unmanagedObj, IntPtr unmanagedObjArgs, Int32 argsTypeIndex, String eventName)\r\n", 
          //                          klass = "ApplicationException", 
          //                          message = "ThrownIntentionally", 
          //                          occured_at = "/Date(1316375205492-0700)/", 
          //                          where = "karfoto:15"
          //                      }
          //                 };


    }
}