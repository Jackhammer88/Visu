using PrecompiledRegex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Prism.Logging;

namespace GcodeParser.GcodeInterpreter
{
    public class GCodePreparer
    {
        private static readonly string[] GCodeOrder = new string[] { "G", "R", "I", "J", "K", "T", "S", "F", "M", "A", "B", "C", "U", "V", "W", "X", "Y", "Z", "D", "E", "H", "L", "N", "O", "P", "Q" };
        private static readonly CNCRegex _regex = new CNCRegex();
        private List<string> _strings;

        public static string Find(string line)
        {
            string result = string.Empty;
            foreach (var match in _regex.Matches(line))
            {
                result += $"{match} ";
            }
            return result;
        }

        public async Task OpenFileAsync(string path, IProgress<float> progressChanger)
        {
            StringsPrepared = false;
            FileName = new FileInfo(path).Name;
            _strings = new List<string>();
            await Task.Run(async () =>
            {
                using var file = new StreamReader(path);
                var wholeTextFile = await file.ReadToEndAsync().ConfigureAwait(false);
                var withoutComments = ReplaceComments(wholeTextFile)
                    .Split('\n').Where(s => !string.IsNullOrEmpty(s) && !s.Equals("\r"))
                    .Select(s => Find(s.Trim('\r', ' ').ToUpperInvariant()));
                _strings.AddRange(withoutComments);
                file.Dispose();
            }).ConfigureAwait(false);
        }

        private string ReplaceComments(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            Regex regex = new Regex(@"\(.+?\)");
            return regex.Replace(input, string.Empty);
        }

        public async Task PrepareStringsAsync()
        {
            await Task.Run(() => _strings = Strings.Select(s => s.Split(' ')
                        .Where(s1 => !string.IsNullOrWhiteSpace(s1))
                            .OrderBy(GcodeSorter)
                                .Aggregate(string.Empty, (total, n) => $"{total} {n}")).ToList()).ConfigureAwait(false);
            StringsPrepared = true;
        }
        private object GcodeSorter(string arg)
        {
            var element = GCodeOrder.First(arg.Contains);
            return Array.IndexOf(GCodeOrder, element);
        }

        public string FileName { get; private set; }

        public IReadOnlyCollection<string> Strings => _strings;
        public bool StringsPrepared { get; private set; }
    }
}
