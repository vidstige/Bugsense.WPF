using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace Bugsense.WPF.tests
{
    class FakeAssemblyRepository : IAssemblyRepository
    {
        public Assembly GetEntryAssembly()
        {
            return new FakeAssembly();
        }
    }

    class FakeAssembly : Assembly
    {
        public override AssemblyName GetName()
        {
            var name = new AssemblyName("FakeAssembly");
            name.Version = new Version("1.2.3.4");
            return name;
        }
    }

    class FakeWebRequest : WebRequest 
    {
        private readonly WebHeaderCollection _headers = new WebHeaderCollection();
        private readonly MemoryStream _requestStream = new MemoryStream();
        private readonly Uri _requestUri;
        private string _actualRequest;        

        public FakeWebRequest(Uri uri)
        {
            _requestUri = uri;
        }

        public override Uri RequestUri { get { return _requestUri; } }

        public override WebHeaderCollection Headers
        {
            get { return _headers; }
        }

        public override string Method
        {
            set { }
        }
        public override string ContentType
        {
            set { }
        }

        public override Stream GetRequestStream()
        {
            return _requestStream;
        }

        public override WebResponse GetResponse()
        {
            _actualRequest = Encoding.UTF8.GetString(_requestStream.ToArray());
            return null;
        }

        public string ActualRequest { get { return _actualRequest; } }
    }

    class FakeWebRequestCreator: IWebRequestCreate
    {
        private readonly IList<FakeWebRequest> _requests = new List<FakeWebRequest>();

        public WebRequest Create(Uri uri)
        {
            var request = new FakeWebRequest(uri);
            _requests.Add(request);
            return request;
        }

        public string GetActualRequest(Uri uri)
        {
            var request = _requests.FirstOrDefault<FakeWebRequest>(r => r.RequestUri == uri);
            Assert.IsNotNull(request, "No request was sent to " + uri);
            return request.ActualRequest;
        }       
    }

    [TestClass]
    public class Given_Exception_is_thrown
    {
        [TestMethod]
        public void When_Sending()
        {
            var uri = new Uri("http://api.example.com/v1/crash");
            var assemblyRepository = new FakeAssemblyRepository();
            var ex = new ArgumentException("message");
            var webRequestCreator = new FakeWebRequestCreator();

            var errorSender = new ErrorSender(null, uri, webRequestCreator);
            errorSender.SendOrStore(new CrashInformationCollector(assemblyRepository, null).CreateCrashReport(ex));
            
            var bugsenseRequest = webRequestCreator.GetActualRequest(uri);
            Assert.IsTrue(bugsenseRequest.StartsWith("data="), "Expected request to start with 'data='");
            var jsonString = Uri.UnescapeDataString(bugsenseRequest.Substring("data=".Length));
            var json = JObject.Parse(jsonString);

            json.Verify("application_environment.appname", "FakeAssembly");
        }
    }
}
