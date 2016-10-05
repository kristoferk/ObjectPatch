using System.Net;

namespace ExampleApiWeb.Api.v1.Contracts
{
    public class Error
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }
    }
}