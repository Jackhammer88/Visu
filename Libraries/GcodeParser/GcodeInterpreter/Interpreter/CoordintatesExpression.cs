using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GcodeParser.GcodeInterpreter.Interpreter
{
    public class CoordintatesExpression : IExpression
    {
        private static readonly string[] Coordinates = new string[] { "A", "B", "C", "U", "V", "W", "X", "Y", "Z" };
        private Context _context;

        public void Interpret(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            var coordinates = _context.InputString.Split(' ').Where(s => Coordinates.Any(c => s.Contains(c)));
            ProcessCoordinates(coordinates);
        }

        private void ProcessCoordinates(IEnumerable<string> coordinates)
        {
            var result = new AxisCoordinates();
            foreach (var coordinate in coordinates)
            {
                ParseCoordinate(new string(coordinate.Take(1).ToArray()), new string(coordinate.Skip(1).ToArray()), result);
            }
            _context.OutputData.Coordinate = result;
        }

        private static void ParseCoordinate(string axisName, string position, AxisCoordinates result)
        {
            var properties = result.GetType();
            var sorted = properties.GetProperties();
            var property = sorted.Single(p => p.Name.Equals(axisName, StringComparison.Ordinal));
            var coordinate = float.Parse(position, CultureInfo.InvariantCulture);
            property.SetValue(result, coordinate);
        }
    }
}
