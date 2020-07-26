using NUnit.Framework;

namespace GcodeParser.GcodeInterpreter.Interpreter.Tests
{
    [TestFixture()]
    public class CoordintatesExpressionTests
    {
        [Test()]
        [TestCase("G1 X0.046 Y0.756 Z-11.451 A0.122 B12.456 C0 U-11.111 V0.2 W8.9")]
        public void InterpretTest(string value)
        {
            var exp = new CoordintatesExpression();
            var context = new Context { InputString = value };

            exp.Interpret(context);

            var coordinates = context.OutputData.Coordinate;
            Assert.IsTrue(coordinates.X == 0.046f && coordinates.Y == 0.756f && coordinates.A == 0.122f && coordinates.B == 12.456f && coordinates.C == 0f && coordinates.U == -11.111f && coordinates.V == 0.2f && coordinates.W == 8.9f);
        }
    }
}