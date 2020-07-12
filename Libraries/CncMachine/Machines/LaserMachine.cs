using System.Globalization;
using System.Linq;
using System.Text;

namespace CncMachine.Machines
{
    public class LaserMachine : MachineBase
    {
        protected new void Initialize()
        {
            base.Initialize();
            CurrentCoordinates = new GcodeParser.AxisCoordinates { X = 0, Y = 0, Z = 0 };
            OldCoordinates = CurrentCoordinates;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (ModalGCodes != null && ModalGCodes.Count > 0)
            {
                if (ModalGCodes.Count > 1)
                {
                    builder.Append(ModalGCodes.Select(c => $"G{c}").Aggregate((s1, s2) => $"{s1} {s2} "));
                }
                else
                    builder.Append($"G{ModalGCodes.First().ToString(CultureInfo.InvariantCulture)} ");
            }
            if (CurrentGCodes != null && CurrentGCodes.Count > 0)
            {
                if (CurrentGCodes.Count > 1)
                    builder.Append(CurrentGCodes.Select(c => $"G{c}").Aggregate((s1, s2) => $"{s1} {s2} "));
                else
                    builder.Append($"G{CurrentGCodes.First().ToString(CultureInfo.InvariantCulture)} ");
            }

            builder.Append($"T{ToolNumber} ");
            builder.Append($"F{FeedRate} ");

            if (ModalMCodes != null && ModalMCodes.Count > 0)
            {
                if (ModalGCodes.Count > 1)
                    builder.Append(ModalMCodes.Select(c => $"M{c}").Aggregate((s1, s2) => $"{s1} {s2} "));
                else
                    builder.Append($"M{ModalMCodes.First().ToString(CultureInfo.InvariantCulture)} ");
            }
            if (CurrentMCodes != null && CurrentMCodes.Count > 0)
            {
                if (CurrentMCodes.Count > 1)
                    builder.Append(CurrentMCodes.Select(c => $"M{c}").Aggregate((s1, s2) => $"{s1} {s2} "));
                else
                    builder.Append($"M{CurrentMCodes.First().ToString(CultureInfo.InvariantCulture)} ");
            }

            var x = CurrentCoordinates.X.HasValue ? $"X{CurrentCoordinates.X.Value} " : string.Empty;
            var y = CurrentCoordinates.Y.HasValue ? $"Y{CurrentCoordinates.Y.Value} " : string.Empty;
            var i = CurrentFrame.IValue.HasValue ? $"I{CurrentFrame.IValue.Value} " : string.Empty;
            var j = CurrentFrame.JValue.HasValue ? $"J{CurrentFrame.JValue.Value} " : string.Empty;
            var r = CurrentFrame.RValue.HasValue ? $"R{CurrentFrame.RValue.Value} " : string.Empty;
            builder.Append($"{x}{y}{i}{j}{r}");

            return builder.ToString();
        }
    }
}
