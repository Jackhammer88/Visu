namespace GcodeParser.GcodeInterpreter.Interpreter
{
    public class Context
    {
        public string InputString { get; set; }
        public GFrame OutputData { get; set; }
        public GFrame PreviousFrame { get; set; }
    }
}