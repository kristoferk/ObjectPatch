using ExampleApiWeb.Api.v1.Contracts;
using ExampleApiWeb.Model;
using System;
using System.Net;

namespace ExampleApiWeb.Code
{
    public class CustomHttpException : Exception
    {
        internal CustomHttpException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
            InnerStackTrace = string.Empty;
        }

        internal CustomHttpException(HttpStatusCode statusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        internal CustomHttpException(string message)
            : base(message)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        internal CustomHttpException(HttpStatusCode statusCode, Error error)
            : base($"One more errors occured. See {nameof(Error)} for more information.")
        {
            StatusCode = statusCode;
            Error = error;
        }

        public HttpStatusCode StatusCode { get; internal set; }

        public string InnerStackTrace { get; internal set; }

        public Uri ResponseUri { get; internal set; }

        public Error Error { get; }
    }
}