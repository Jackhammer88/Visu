using System;

namespace GcodeParser.Exceptions
{
    [Serializable]
    public class GCodeParserException : Exception
    {
        public GCodeParserException() { }
        public GCodeParserException(string message) : base(message) { }

        public GCodeParserException(string message, Exception innerException) : base(message, innerException) { }

        protected GCodeParserException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
    }
}