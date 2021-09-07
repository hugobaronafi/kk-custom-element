using System;
using System.Runtime.Serialization;

namespace Core
{
    [Serializable]
    public class ApiException : Exception
    {
        public bool Skip { get; }

        public ApiException()
        {
        }

        public ApiException(string? message, bool skip = false) : base(message)
        {
            Skip = skip;
        }

        public ApiException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}