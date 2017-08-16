using System;

namespace App.Template.XForms.Core.Exceptions
{
    [Serializable]
    public class HttpException : Exception
    {
        public HttpException()
        {
        }

        public HttpException(string message)
            : base(message)
        {
        }

        public HttpException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public HttpException(int httpCode, string message) : base(message)
        {
            ErrorCode = httpCode;
        }

        public HttpException(int httpCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = httpCode;
        }
        public int ErrorCode { get; }
    }
}