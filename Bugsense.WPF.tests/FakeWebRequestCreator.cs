using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Bugsense.WPF.tests
{
    class FakeWebRequestCreator : IWebRequestCreate
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
}
