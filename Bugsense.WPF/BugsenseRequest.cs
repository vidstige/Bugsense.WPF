using System;
using System.Runtime.Serialization;

namespace Bugsense.WPF
{
    [DataContract]
    public class BugSenseEx
    {
        internal Exception OriginalException { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
        [DataMember(Name = "backtrace")]
        public string StackTrace { get; set; }
        [DataMember(Name = "occured_at")]
        public DateTime DateOccured { get; set; }
        [DataMember(Name = "klass")]
        public string ExceptionType { get; set; }
        //[DataMember(Name = "where")]
        //public string Where { get; set; }
        //public string Comment { get; set; }
    }

    [DataContract]
    public class AppEnvironment
    {
        [DataMember(Name = "phone")]
        public string PhoneModel { get; set; }
        [DataMember(Name = "appver")]
        public string AppVersion { get; set; }
        [DataMember(Name = "appname")]
        public string AppName { get; set; }
        [DataMember(Name = "osver")]
        public string OsVersion { get; set; }
        //[DataMember(Name = "wifi_on")]
        //public string WifiOn { get; set; }
        //[DataMember(Name = "gps_on")]
        //public string GpsOn { get; set; }
        //[DataMember(Name = "screen:width")]
        //public double ScreenWidth { get; set; }
        //[DataMember(Name = "screen:height")]
        //public double ScreenHeight { get; set; }
        //[DataMember(Name = "screen:orientation")]
        //public string ScreenOrientation { get; set; }
        //[DataMember(Name = "screen_dpi(x:y)")]
        //public string ScreenDpi { get; set; }
    }

    [DataContract]
    public class BugSenseRequest
    {
        public BugSenseRequest() { }
        public BugSenseRequest(BugSenseEx ex, AppEnvironment environment, BugSenseClient bugSenseClient)
        {
            Client = new BugSenseClient();
            Request = new BugSenseInternalRequest();
            //Request.Comment = string.IsNullOrEmpty(ex.Comment) ? ex.Message : ex.Comment;
            Exception = ex;
            AppEnvironment = environment;
        }
        [DataMember(Name = "exception")]
        public BugSenseEx Exception { get; set; }
        [DataMember(Name = "application_environment")]
        public AppEnvironment AppEnvironment { get; set; }
        [DataMember(Name = "client")]
        public BugSenseClient Client { get; set; }
        [DataMember(Name = "request")]
        public BugSenseInternalRequest Request { get; set; }
    }

    [DataContract]
    public class BugSenseClient
    {

        public BugSenseClient()
        {
            //Version = "bugsense-version-0.6";
            //Name = "bugsense-wp7";
        }

        [DataMember(Name = "version")]
        public string Version { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    [DataContract]
    public class BugSenseInternalRequest
    {
        [DataMember(Name = "comment")]
        public string Comment { get; set; }
    }
}
