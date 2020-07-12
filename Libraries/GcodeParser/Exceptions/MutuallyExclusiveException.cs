using System;

namespace GcodeParser.Exceptions
{
    [Serializable]
    public class MutuallyExclusiveException : GCodeParserException
    {
        public MutuallyExclusiveException() { }
        public MutuallyExclusiveException(string message) : base(message) { }

        public MutuallyExclusiveException(string message, Exception innerException) : base(message, innerException) { }

        protected MutuallyExclusiveException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
    }
}
