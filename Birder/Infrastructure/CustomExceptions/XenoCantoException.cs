using System.Net;

namespace Birder.Infrastructure.CustomExceptions
{
    public class XenoCantoException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public XenoCantoException() { }

        public XenoCantoException(HttpStatusCode statusCode)
            => StatusCode = statusCode;

        public XenoCantoException(HttpStatusCode statusCode, string message) : base(message)
            => StatusCode = statusCode;

        public XenoCantoException(HttpStatusCode statusCode, string message, Exception inner) : base(message, inner)
            => StatusCode = statusCode;
    }
}
