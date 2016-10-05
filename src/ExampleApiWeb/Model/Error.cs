using SimpleObjectPatch;
using System;
using System.Net;

namespace ExampleApiWeb.Model
{
    public class Error
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }
    }

}