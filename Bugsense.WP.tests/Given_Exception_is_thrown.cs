using Bugsense.WPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace Bugsense.WP.tests
{
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
            var request = new Mock<WebRequest>();
            request.SetupAllProperties();
            request.Object.Headers = new WebHeaderCollection();
            var result = new MemoryStream();
            request.Setup(r => r.GetRequestStream()).Returns(result);

                        var webRequestCreator = new Mock<IWebRequestCreate>();
            webRequestCreator.Setup(c => c.Create(uri)).Returns(request.Object);

            var assemblyRepository = new Mock<IAssemblyRepository>();
            assemblyRepository.Setup(r => r.GetEntryAssembly()).Returns(new FakeAssembly());

            var ex = new ArgumentException("message");

            var errorSender = new ErrorSender(null, uri, webRequestCreator.Object);
            errorSender.SendOrStore(new CrashInformationCollector(assemblyRepository.Object, null).CreateCrashReport(ex));
        }
    }
}
