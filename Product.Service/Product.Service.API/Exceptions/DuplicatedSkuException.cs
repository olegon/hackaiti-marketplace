namespace Product.Service.API.Exceptions
{
    [System.Serializable]
    public class DuplicatedSkuException : System.Exception
    {
        public DuplicatedSkuException() { }
        public DuplicatedSkuException(string message) : base(message) { }
        public DuplicatedSkuException(string message, System.Exception inner) : base(message, inner) { }
        protected DuplicatedSkuException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}