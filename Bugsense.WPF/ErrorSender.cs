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
        private readonly string _apiKey;
        private readonly Uri _apiUri;
        private readonly IWebRequestCreate _webRequestCreator;

        public ErrorSender(string apiKey, Uri apiUri, IWebRequestCreate webRequestCreator)
        {
            _apiKey = apiKey;
            _apiUri = apiUri;
            _webRequestCreator = webRequestCreator;
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
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        private void Send(string serializedErrorReport)
        {
            var request = _webRequestCreator.Create(_apiUri);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers["X-BugSense-Api-Key"] = _apiKey;
            
            using (var stream = request.GetRequestStream())
            using (var requestStream = new StreamWriter(stream))
            {
                requestStream.Write(serializedErrorReport);
            }

            var response = request.GetResponse();

            //var content = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //Console.WriteLine(content);
            //foreach (var header in response.Headers)
            //{
            //    Console.WriteLine(header);
            //}
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