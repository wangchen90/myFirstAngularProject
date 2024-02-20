using System;
using System.Net;
using System.Runtime.Serialization;

namespace UserRegistrationAPI.Controllers
{
    [Serializable]
    internal class HttpResponseException : Exception
    {
        private HttpStatusCode notFound;
        private string v;

        public HttpResponseException()
        {
        }

        public HttpResponseException(string message) : base(message)
        {
        }

        public HttpResponseException(HttpStatusCode notFound, string v)
        {
            this.notFound = notFound;
            this.v = v;
        }

        public HttpResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HttpResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}