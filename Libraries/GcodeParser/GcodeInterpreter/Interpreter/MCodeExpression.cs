using GcodeParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GcodeParser.GcodeInterpreter.Interpreter
{
    public class MCodeExpression : IExpression
    {
        public static readonly float[][] MutuallyExclusiveCodes = new float[][]
        {
            new float[] { 0, 1, 2, 30, 60 },
            new float[] { 3, 4, 5 },
            new float[] { 7, 8, 9 },
            new float[] { 48, 49 }
        };
        Context _context;

        public void Interpret(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            var mcodes = context.InputString.Split(' ').Where(f => f.Contains("M"));
            MCodeProcess(mcodes);

            if (_context.OutputData.MCodes.Count > 1) CheckForMutuallyExclusiveCodes(_context.OutputData.MCodes);
        }

        private void CheckForMutuallyExclusiveCodes(HashSet<float> mCodes)
        {
            //int repeatCount = 0;
            foreach (var codeGroup in MutuallyExclusiveCodes)
            {
                if (codeGroup.Intersect(mCodes).Count() > 1)
                    throw new MutuallyExclusiveException($"M-Group: {codeGroup.Select(e => e.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => $"{s1} {s2}")}");
                //foreach (var groupElement in codeGroup)
                //{

                //    if (mCodes.Any(e => groupElement == e))
                //        repeatCount++;
                //    if (repeatCount > 1)
                //        throw new MutuallyExclusiveException($"M-Group: {codeGroup.Select(e => e.ToString(CultureInfo.InvariantCulture)).Aggregate((s1, s2) => $"{s1} {s2}")}");
                //}
                //repeatCount = 0;
            }
        }

        private void MCodeProcess(IEnumerable<string> mcodes)
        {
            var result = new HashSet<float>();
            foreach (var mcode in mcodes)
            {
                result.Add(float.Parse(new string(mcode.Skip(1).ToArray()), CultureInfo.InvariantCulture));
            }
            _context.OutputData.MCodes.Clear();
            _context.OutputData.MCodes.UnionWith(result);
        }
    }
}
