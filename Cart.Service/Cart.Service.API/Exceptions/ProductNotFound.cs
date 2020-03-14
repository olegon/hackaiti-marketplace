using System;
using System.Runtime.Serialization;

namespace Cart.Service.API.Exceptions
{
    [Serializable]
    internal class ProductNotFound : Exception
    {
        public ProductNotFound()
        {
        }

        public ProductNotFound(string message) : base(message)
        {
        }

        public ProductNotFound(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}