using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security;
using System.Text;
using System;

namespace Bugsense.WPF
{
    class ErrorSender
    {
        private readonly string apiKey;
        private readonly string apiUrl;

        public ErrorSender(string apiKey, string apiUrl)
        {
            this.apiKey = apiKey;
            this.apiUrl = apiUrl;
        }

        private string ToJsonString<T>(T o)
        {
            var ms = new MemoryStream();
            var jsonSerializer = new DataContractJsonSerializer(typeof(T));
            jsonSerializer.WriteObject(ms, o);
            return Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
        }

        internal void SendOrStore(BugSenseRequest errorReport)
        {
            string serializedErrorReport = Serialize(errorReport);
            try
            {
                Send(serializedErrorReport);
            }
            catch (IOException)
            {
                Store(serializedErrorReport);
            }
            catch (SecurityException)
            {
                Store(serializedErrorReport);
            }
        }

        private string Serialize(BugSenseRequest errorReport)
        {
            var ms = new MemoryStream();
            using (var requestStream = new StreamWriter(ms))
            {
                requestStream.Write("data=");
                requestStream.Write(Uri.EscapeDataString(ToJsonString(errorReport)));
            }
            return ms.ToString();
        }

        private void Send(string serializedErrorReport)
        {
            var request = WebRequest.Create(apiUrl);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers["X-BugSense-Api-Key"] = apiKey;
            
            using (var stream = request.GetRequestStream())
            using (var requestStream = new StreamWriter(stream))
            {
                requestStream.Write(serializedErrorReport);
            }

            var response = request.GetResponse();

            // TODO: Handle response...?

            //string text = string.Empty;
            //var responseStream = response.GetResponseStream();
            //if (responseStream != null)
            //{
            //    var streamReader = new StreamReader(responseStream);
            //    text = streamReader.ReadToEnd();
            //    streamReader.Close();
            //}
        }

        private void Store(string serializedErrorReport)
        {
        }
    }
}