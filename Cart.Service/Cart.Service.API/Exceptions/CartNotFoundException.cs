using System;
using System.Runtime.Serialization;

namespace Cart.Service.API.Exceptions
{
    [Serializable]
    internal class CartNotFoundException : BusinessException
    {
        public CartNotFoundException()
        {
        }

        public CartNotFoundException(string message) : base(message)
        {
        }

        public CartNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CartNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}