using System.Runtime.Serialization;

namespace MyBlog.Middleware
{
    [Serializable]
    internal class YourAppException : Exception
    {
        public YourAppException()
        {
        }

        public YourAppException(string? message) : base(message)
        {
        }

        public YourAppException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected YourAppException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}