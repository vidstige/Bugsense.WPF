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

        public ErrorSender(string apiKey)
        {
            this.apiKey = apiKey;
        }

        internal void Send(BugSenseRequest errorReport)
        {
            const string baseUrl = "http://bugsense.appspot.com/api/errors";
            var request = WebRequest.Create(baseUrl);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.Headers[HttpRequestHeader.UserAgent] = "WP7";
            //request.Headers["User-Agent"] = "WP7";

            request.Headers["X-BugSense-Api-Key"] = apiKey;

            var stream = request.GetRequestStream();
            var json = new MemoryStream();
            using (var requestStream = new StreamWriter(stream))
            {
                //requestStream.Write("data=" + Uri.EscapeUriString(errorJson));
                
                requestStream.Write("data=");
                requestStream.Flush();

                var jsonSerializer = new DataContractJsonSerializer(typeof(BugSenseRequest));
                jsonSerializer.WriteObject(json, errorReport);
                var jsonString = Encoding.UTF8.GetString(json.GetBuffer(), 0, (int) json.Length);
                requestStream.Write(Uri.EscapeDataString(jsonString));
            }
            stream.Close();

            var response = request.GetResponse();

            string text = string.Empty;
            var responseStream = response.GetResponseStream();
            if (responseStream != null)
            {
                var streamReader = new StreamReader(responseStream);
                text = streamReader.ReadToEnd();
                streamReader.Close();
            }
        }
    }
}