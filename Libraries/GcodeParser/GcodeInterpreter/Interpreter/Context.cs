using Infrastructure.Abstract.GCode;

namespace GcodeParser.GcodeInterpreter.Interpreter
{
    public class Context
    {
        public Context()
        {
            OutputData = new GFrame();
        }
        public string InputString { get; set; }
        public GFrame OutputData { get; set; }
        public GFrame PreviousFrame { get; set; }
    }
}