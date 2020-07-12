using GcodeParser.Exceptions;
using GcodeParser.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace GcodeParser.GcodeInterpreter.Interpreter
{
    public class GCodeExpression : IExpression
    {
        public GCodeExpression()
        {

        }
        private static readonly string[] AdditionalValues = new string[] { "I", "J", "K", "R" };
        public static readonly float[][] MutuallyExclusiveCodes = new float[][]
        {
            new float[] { 0, 1, 2, 3, 77, 78, 80, 81, 82, 84, 85, 86, 87, 88, 89 }, new float[] { 17, 18 , 19 }, //motion
            new float[] { 17, 18, 19 }, //plane selection – XY, YZ, ZX
            new float[] { 90, 91 }, //absolute/incremental mode
            new float[] { 93, 94 }, //feed rate mode
            new float[] { 20, 21 }, //units – inches/millimeters
            new float[] { 40, 41, 42 }, //cutter radius compensation – CRC
            new float[] { 43, 49 }, //tool length offset – TLO
            new float[] { 98, 99 }, //return mode in canned cycles
            new float[] { 54, 55, 56, 57, 58, 59 } //work coordinate system selection – WCSS
        };
        private Context _context;

        public void Interpret(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            var elements = context.InputString.Split(' ');
            var gcodes = elements.Where(s => s.Contains("G"));
            GCodeParse(gcodes);
            if (!_context.OutputData.GCodes.Any(c => c == 0 || c == 1 || c == 2 || c == 3)
                && _context.PreviousFrame != null
                && _context.PreviousFrame.GCodes.Any(c => c == 0 || c == 1 || c == 2 || c == 3))
            {
                var code = _context.PreviousFrame.GCodes.Single(c => c == 0 || c == 1 || c == 2 || c == 3);
                _context.OutputData.GCodes.Add(code);
            }
            if (_context.OutputData.GCodes.Count > 1) CheckForMutuallyExclusiveCodes(_context.OutputData.GCodes);
            if (_context.OutputData.GCodes.Any(e => e == 2 || e == 3))
            {
                if (elements.Any(e => e.Contains("R")))
                    _context.OutputData.RValue = float.Parse(new string(elements.First(e => e.Contains("R")).Skip(1).ToArray()), CultureInfo.InvariantCulture);
                else
                {
                    var ijkr = context.InputString.Split(' ').Where(s => AdditionalValues.Any(v => s.Contains(v)));
                    try
                    {
                        IJKRParse(ijkr);
                    }
                    catch (NotEnoughArgumentsException)
                    {
                        Debug.WriteLine(_context.InputString);
                    }
                }
            }
        }

        private void CheckForMutuallyExclusiveCodes(IEnumerable<float> gCodes)
        {
            int repeatCount = 0;
            foreach (var codeGroup in MutuallyExclusiveCodes)
            {
                foreach (var groupElement in codeGroup)
                {
                    if (gCodes.Any(e => groupElement == e))
                        repeatCount++;
                    if (repeatCount > 1)
                        throw new MutuallyExclusiveException($"Group: {codeGroup.Select(e => e.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => $"{s1} {s2}")}");
                }
                repeatCount = 0;
            }
        }

        private void GCodeParse(IEnumerable<string> codes)
        {
            var result = new HashSet<float>();
            _context.OutputData = new GFrame();
            foreach (var code in codes)
            {
                result.Add(float.Parse(new string(code.Skip(1).ToArray()), CultureInfo.InvariantCulture));
            }
            _context.OutputData.GCodes.Clear();
            _context.OutputData.GCodes.UnionWith(result);
        }
        private void IJKRParse(IEnumerable<string> ijkr)
        {
            var i = ijkr.FirstOrDefault(s => s.Contains("I"));
            var j = ijkr.FirstOrDefault(s => s.Contains("J"));
            var k = ijkr.FirstOrDefault(s => s.Contains("K"));
            var r = ijkr.FirstOrDefault(s => s.Contains("R"));
            if (i == null && j == null && k == null && r == null)
                throw new NotEnoughArgumentsException(CommonStrings.CantCalculateArcCode);
            if (i != null)
                _context.OutputData.IValue = float.Parse(new string(i.Skip(1).ToArray()), CultureInfo.InvariantCulture);
            if (j != null)
                _context.OutputData.JValue = float.Parse(new string(j.Skip(1).ToArray()), CultureInfo.InvariantCulture);
            if (k != null)
                _context.OutputData.KValue = float.Parse(new string(k.Skip(1).ToArray()), CultureInfo.InvariantCulture);
            if (r != null)
                _context.OutputData.RValue = float.Parse(new string(r.Skip(1).ToArray()), CultureInfo.InvariantCulture);
        }
    }

}