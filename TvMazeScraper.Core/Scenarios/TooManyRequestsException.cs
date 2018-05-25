using System;
using System.Runtime.Serialization;

namespace TvMazeScraper.Core.Scenarios
{
    public class TooManyRequestsException : Exception
    {
        public TooManyRequestsException() {}

        public TooManyRequestsException(string message) : base(message) { }

        public TooManyRequestsException(string message, Exception inner) : base(message, inner) { }

        protected TooManyRequestsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}