using System;
using System.Net;

namespace Bugsense.WPF
{
    class WebRequestCreator : IWebRequestCreate
    {
        public WebRequest Create(Uri uri)
        {
            return WebRequest.Create(uri);
        }
    }
}
