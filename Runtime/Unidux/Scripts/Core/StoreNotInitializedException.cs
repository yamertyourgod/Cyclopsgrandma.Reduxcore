using System;
using System.Runtime.Serialization;

namespace Unidux
{
    [Serializable]
    internal class StoreNotInitializedException : Exception
    {
        public StoreNotInitializedException()
        {
        }

        public StoreNotInitializedException(string message) : base(message)
        {
        }

        public StoreNotInitializedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StoreNotInitializedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}