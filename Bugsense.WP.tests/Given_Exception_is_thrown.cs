using Bugsense.WPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net;

namespace Bugsense.WP.tests
{
    [TestClass]
    public class Given_Exception_is_thrown
    {
        [TestMethod]
        public void Foobar()
        {
            var webRequestCreator = new Mock<IWebRequestCreate>();
            var errorSender = new ErrorSender(null, null, webRequestCreator.Object);
            errorSender.SendOrStore(new BugSenseRequest(new BugSenseEx(), new AppEnvironment()));
        }
    }
}
