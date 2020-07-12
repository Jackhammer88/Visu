using System;

namespace GcodeParser.Exceptions
{
    [Serializable]
    public class NotEnoughArgumentsException : GCodeParserException
    {
        public NotEnoughArgumentsException() { }
        public NotEnoughArgumentsException(string message) : base(message) { }

        public NotEnoughArgumentsException(string message, Exception innerException) : base(message, innerException) { }

        protected NotEnoughArgumentsException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

    }
}
