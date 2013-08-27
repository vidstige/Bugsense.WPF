using Newtonsoft.Json.Linq;

namespace Bugsense.WPF.tests
{
    static class JsonUnitTestExtensions
    {
        public static void Verify(this JObject root, string path, string expectedValue)
        {
            var jsonNode = root.Descendants().Single(n => n.Path == path);
            Assert.AreEqual(expectedValue, jsonNode.ToString());
        }
    }
}
