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

    [TestClass]
    public class Given_Exception_is_thrown
    {
        private Uri _uri; 
        private FakeWebRequestCreator _webRequestCreator;
        private readonly FakeAssemblyRepository assemblyRepository = new FakeAssemblyRepository();

        [TestInitialize]
        public void Init()
        {
            _uri = new Uri("http://api.example.com/v1/crash");
            _webRequestCreator = new FakeWebRequestCreator();
        }

        [TestMethod]
        public void AppName_is_entry_assemblyname()
        {
            SendOrStore(new ArgumentException("message"));

            var json = GetSentJson();
            json.Verify("application_environment.appname", "FakeAssembly");
        }

        [TestMethod]
        public void Appver_is_set()
        {
            SendOrStore(new ArgumentException("message"));

            var json = GetSentJson();
            json.Verify("application_environment.appver", "1.2.3.4");
        }

        [TestMethod]
        public void Exception_class_name_is_set()
        {
            SendOrStore(new ArgumentException("message"));

            var json = GetSentJson();
            json.Verify("exception.klass", "System.ArgumentException");
        }

        [TestMethod]
        public void Exception_message_is_set()
        {
            SendOrStore(new ArgumentException("my message"));

            var json = GetSentJson();
            json.Verify("exception.message", "my message");
        }

        private void SendOrStore(Exception ex)
        {
            var errorSender = new ErrorSender(null, _uri, _webRequestCreator);
            errorSender.SendOrStore(new CrashInformationCollector(assemblyRepository, null).CreateCrashReport(ex));
        }

        private JObject GetSentJson()
        {
            var bugsenseRequest = _webRequestCreator.GetActualRequest(_uri);
            Assert.IsTrue(bugsenseRequest.StartsWith("data="), "Expected request to start with 'data='");
            var jsonString = Uri.UnescapeDataString(bugsenseRequest.Substring("data=".Length));
            return JObject.Parse(jsonString);
        }
    }
}
