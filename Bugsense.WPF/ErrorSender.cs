using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;

namespace Bugsense.WPF
{
    class ErrorSender
    {
        public ErrorSender(string apiKey)
        {
            
        }

        internal void Send(ErrorReport errorReport)
        {
            object webRequest = new
            {
                application_environment = new
                {
                    appname = "Rebtel Phone",
                    appver = "1.1.0.0",
                    gps_on = "unvailable",
                    osver = "7.0.7389",
                    phone = "Windows 7",
                    //screen_height = 800,
                    //screen_width = 480,
                    //screen_orientation = "unavailable",
                    wifi_on = "None"
                },
                client = new
                {
                    name = "bugsense-wp7",
                    version = "bugsense-version-0.6"
                },
                exception = new
                {
                    backtrace = "at BugSenseTestApp.MainPage.Button_Click(Object sender, RoutedEventArgs e)\r\n   at System.Windows.Controls.Primitives.ButtonBase.OnClick()\r\n   at System.Windows.Controls.Button.OnClick()\r\n   at System.Windows.Controls.Primitives.ButtonBase.OnMouseLeftButtonUp(MouseButtonEventArgs e)\r\n   at System.Windows.Controls.Control.OnMouseLeftButtonUp(Control ctrl, EventArgs e)\r\n   at MS.Internal.JoltHelper.FireEvent(IntPtr unmanagedObj, IntPtr unmanagedObjArgs, Int32 argsTypeIndex, String eventName)\r\n",
                    klass = "ApplicationException",
                    message = "ThrownIntentionally",
                    occured_at = "/Date(1316375205492-0700)/",
                    where = "karfoto:15"
                }
            };

            //var errorJson = JsonConvert.SerializeObject(webRequest);

            const string apiKey = "f7be3d04";

            const string baseUrl = "http://bugsense.appspot.com/api/errors";
            var request = WebRequest.Create(baseUrl);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.Headers[HttpRequestHeader.UserAgent] = "WP7";
            //request.Headers["User-Agent"] = "WP7";

            request.Headers["X-BugSense-Api-Key"] = apiKey;

            //var stream = request.GetRequestStream();
            var stream = new MemoryStream();
            using (var requestStream = new StreamWriter(stream))
            {
                //requestStream.Write("data=" + Uri.EscapeUriString(errorJson));
                requestStream.Write("data=");
                var jsonSerializer = new DataContractJsonSerializer(typeof(BugSenseRequest));
                jsonSerializer.WriteObject(stream, webRequest);
            }
            
            var streamReader = new StreamReader(stream);
            var text = streamReader.ReadToEnd();
            streamReader.Close();
            

            //var response = request.GetResponse();

            //string text = string.Empty;
            //var responseStream = response.GetResponseStream();
            //if (responseStream != null)
            //{
            //    var streamReader = new StreamReader(responseStream);
            //    text = streamReader.ReadToEnd();
            //    streamReader.Close();
            //}
            
        }
    }
}