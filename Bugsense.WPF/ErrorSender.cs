using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
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

        internal void Send(BugSenseRequest errorReport)
        {
            var request = WebRequest.Create(apiUrl);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers["X-BugSense-Api-Key"] = apiKey;
            var stream = request.GetRequestStream();
            
            using (var requestStream = new StreamWriter(stream))
            {
                requestStream.Write("data=");
                requestStream.Flush();
                requestStream.Write(Uri.EscapeDataString(ToJsonString(errorReport)));
            }
            stream.Close();

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
    }
}