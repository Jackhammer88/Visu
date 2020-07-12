using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GcodeParser
{
    public class GFrame
    {
        public GFrame()
        {
            MCodes = new HashSet<float>();
            GCodes = new HashSet<float>();
        }
        public AxisCoordinates Coordinate { get; set; }
        public HashSet<float> MCodes { get; }
        public HashSet<float> GCodes { get; }
        public float? ToolNumber { get; set; }
        public float? Feedrate { get; set; }
        public float? RValue { get; set; }
        public float? IValue { get; set; }
        public float? JValue { get; set; }
        public float? KValue { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (GCodes != null && GCodes.Count > 0)
            {
                if (GCodes.Count > 1)
                    builder.Append($"{GCodes.Select(c => c.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => $"G{s1} G{s2} ")}");
                else
                    builder.Append($"G{GCodes.First().ToString(CultureInfo.InvariantCulture)} ");
            }
            if (MCodes != null && MCodes.Count > 0)
            {
                if (MCodes.Count > 1)
                    builder.Append($"{MCodes.Select(c => c.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => $"M{s1} M{s2} ")}");
                else
                    builder.Append($"M{MCodes.First().ToString(CultureInfo.InvariantCulture)} ");
            }
            if (Coordinate.X.HasValue)
            {
                builder.Append($"X{Coordinate.X.Value} ");
                if (IValue.HasValue)
                    builder.Append($"I{IValue} ");
            }
            if (Coordinate.Y.HasValue)
            {
                builder.Append($"Y{Coordinate.Y.Value} ");
                if (JValue.HasValue)
                    builder.Append($"J{JValue} ");
            }
            if (Coordinate.Z.HasValue)
            {
                builder.Append($"Z{Coordinate.Z.Value} ");
                if (KValue.HasValue)
                    builder.Append($"K{KValue} ");
            }
            if (RValue.HasValue)
                builder.Append($"R{RValue} ");
            if (Feedrate.HasValue)
                builder.Append($"F{Feedrate} ");
            if (ToolNumber.HasValue)
                builder.Append($"T{ToolNumber} ");
            return builder.ToString();
        }
    }
}