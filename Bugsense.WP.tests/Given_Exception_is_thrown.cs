using Bugsense.WPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace Bugsense.WP.tests
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
            return new MemoryStream();
        }

        public override WebResponse GetResponse()
        {
            return null;
        }
    }

    class FakeWebRequestCreator: IWebRequestCreate
    {
        public WebRequest Create(Uri uri)
        {
            return new FakeWebRequest();
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
        }
    }
}
