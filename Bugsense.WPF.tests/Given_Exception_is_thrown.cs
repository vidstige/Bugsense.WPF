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
