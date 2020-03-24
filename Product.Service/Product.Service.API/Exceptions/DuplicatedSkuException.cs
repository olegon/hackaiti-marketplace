using System;
using System.Runtime.Serialization;

namespace Product.Service.API.Exceptions
{
    [Serializable]
    public class DuplicatedSkuException : BusinessException
    {
        public DuplicatedSkuException()
        {
        }

        public DuplicatedSkuException(string message) : base(message)
        {
        }

        public DuplicatedSkuException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicatedSkuException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}