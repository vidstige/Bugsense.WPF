using System;
using System.IO;
using System.Net;
using System.Text;

namespace Bugsense.WPF.tests
{
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
}
