using System;
using System.Globalization;
using System.Linq;

namespace GcodeParser.GcodeInterpreter.Interpreter
{
    public class TCodeExpression : IExpression
    {
        private Context _context;

        public void Interpret(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            var tCode = context.InputString.Split(' ').FirstOrDefault(f => f.Contains("T"));
            if (tCode != null)
                _context.OutputData.ToolNumber = float.Parse(new string(tCode.Skip(1).ToArray()), CultureInfo.InvariantCulture);
        }
    }
}
