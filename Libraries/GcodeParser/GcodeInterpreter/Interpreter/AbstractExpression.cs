namespace GcodeParser.GcodeInterpreter.Interpreter
{
    public interface IExpression
    {
        void Interpret(Context context);
    }
}