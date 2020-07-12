using System;
using System.Globalization;
using System.Linq;

namespace GcodeParser.GcodeInterpreter.Interpreter
{
    public class FeedrateExpression : IExpression
    {
        public void Interpret(Context context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var elements = context.InputString.Split(' ');
            var feedrate = elements.FirstOrDefault(s => s.Contains("F"));
            if (feedrate != null) context.OutputData.Feedrate = float.Parse(new string(feedrate.Skip(1).ToArray()), CultureInfo.InvariantCulture);
        }
    }
}
