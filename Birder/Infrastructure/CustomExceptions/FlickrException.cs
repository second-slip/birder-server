using System.Net;

namespace Birder.Infrastructure.CustomExceptions
{
    public class FlickrException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public FlickrException() { }

        public FlickrException(HttpStatusCode statusCode)
            => StatusCode = statusCode;

        public FlickrException(HttpStatusCode statusCode, string message) : base(message)
            => StatusCode = statusCode;

        public FlickrException(HttpStatusCode statusCode, string message, Exception inner) : base(message, inner)
            => StatusCode = statusCode;
    }
}
