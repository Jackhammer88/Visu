using System.Globalization;
using System.Linq;

namespace CncMachine.Machines
{
    public class MillMachine : MachineBase
    {
        public MillMachine()
        {

        }
        protected new void Initialize()
        {
            base.Initialize();
            CurrentCoordinates = new GcodeParser.AxisCoordinates { X = 0, Y = 0, Z = 0 };
            OldCoordinates = CurrentCoordinates;
        }

        public override string ToString()
        {
            var modalGCodes = ModalGCodes.Count > 0 ? $"{ModalGCodes.Select(code => code.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => string.IsNullOrEmpty(s2) ? $"G{s1} G{s2}" : $"G{s1}")} " : string.Empty;
            var nonModalGCodes = CurrentGCodes.Count > 0 ? $"{CurrentGCodes.Select(code => code.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => string.IsNullOrEmpty(s2) ? $"G{s1} G{s2}" : $"G{s1}")} " : string.Empty;
            var modalMCodes = ModalMCodes.Count > 0 ? $"{ModalMCodes.Select(code => code.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => string.IsNullOrEmpty(s2) ? $"M{s1} M{s2}" : $"M{s1}")} " : string.Empty;
            var nonModalMCodes = CurrentMCodes.Count > 0 ? $"{CurrentMCodes.Select(code => code.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => string.IsNullOrEmpty(s2) ? $"M{s1} M{s2}" : $"M{s1}")} " : string.Empty;
            var x = CurrentCoordinates.X.HasValue ? $"X{CurrentCoordinates.X.Value} " : string.Empty;
            var y = CurrentCoordinates.Y.HasValue ? $"Y{CurrentCoordinates.Y.Value} " : string.Empty;
            var z = CurrentCoordinates.Z.HasValue ? $"Z{CurrentCoordinates.Z.Value} " : string.Empty;
            var a = CurrentCoordinates.A.HasValue ? $"A{CurrentCoordinates.A.Value} " : string.Empty;
            var b = CurrentCoordinates.B.HasValue ? $"B{CurrentCoordinates.B.Value} " : string.Empty;
            var c = CurrentCoordinates.C.HasValue ? $"C{CurrentCoordinates.C.Value} " : string.Empty;
            var u = CurrentCoordinates.U.HasValue ? $"U{CurrentCoordinates.U.Value} " : string.Empty;
            var v = CurrentCoordinates.V.HasValue ? $"V{CurrentCoordinates.V.Value} " : string.Empty;
            var w = CurrentCoordinates.W.HasValue ? $"W{CurrentCoordinates.W.Value} " : string.Empty;
            var i = CurrentFrame.IValue.HasValue ? $"I{CurrentFrame.IValue.Value} " : string.Empty;
            var j = CurrentFrame.JValue.HasValue ? $"J{CurrentFrame.JValue.Value} " : string.Empty;
            var k = CurrentFrame.KValue.HasValue ? $"K{CurrentFrame.KValue.Value} " : string.Empty;
            return $"{modalGCodes}{nonModalGCodes} T{ToolNumber} F{FeedRate} {modalMCodes}{nonModalMCodes}{x}{y}{z}{a}{b}{c}{u}{v}{w}{i}{j}{k}";
        }
    }
}
