using PrecompiledRegex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GcodeParser.GcodeInterpreter
{
    public class GCodePreparer
    {
        private readonly static string[] GCodeOrder = new string[] { "G", "R", "I", "J", "K", "T", "S", "F", "M", "A", "B", "C", "U", "V", "W", "X", "Y", "Z", "D", "E", "H", "L", "N", "O", "P", "Q" };
        private static readonly CNCRegex _regex = new CNCRegex();

        public void OpenFile(string path)
        {
            StringsPrepared = false;
            FileName = new FileInfo(path).Name;
            Strings = new List<string>();
            using (var file = new StreamReader(path))
            {
                Strings.AddRange(file.ReadToEnd().Split('\n').Select(s => Find(s.Trim('\r').Trim(' ').ToUpperInvariant())));
            }
        }

        public static string Find(string line)
        {
            string result = string.Empty;
            foreach (var match in _regex.Matches(line))
            {
                result += $"{match} ";
            }
            return result;
        }

        public async Task OpenFileAsync(string path)
        {
            StringsPrepared = false;
            FileName = new FileInfo(path).Name;
            Strings = new List<string>();
            await Task.Run(async () =>
            {
                using (var file = new StreamReader(path)) Strings.AddRange((await file.ReadToEndAsync().ConfigureAwait(false)).Split('\n').Select(s => Find(s.Trim('\r').Trim(' ').ToUpperInvariant())));
            }).ConfigureAwait(false);
        }
        public void PrepareStrings()
        {
            Strings = Strings.Select(s => ExcludeComments(s))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.Split(' ')
                        .Where(s1 => !string.IsNullOrWhiteSpace(s1))
                            .OrderBy(GcodeSorter)
                                .Aggregate((s1, s2) => $"{s1} {s2}")).ToList();
            StringsPrepared = true;
        }
        public async Task PrepareStringsAsync()
        {
            await Task.Run(() => Strings = Strings.Select(s => ExcludeComments(s))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.Split(' ')
                        .Where(s1 => !string.IsNullOrWhiteSpace(s1))
                            .OrderBy(GcodeSorter)
                                .Aggregate((s1, s2) => $"{s1} {s2}")).ToList()).ConfigureAwait(false);
            StringsPrepared = true;
        }
        private object GcodeSorter(string arg)
        {
            var element = GCodeOrder.First(s => arg.Contains(s));
            return Array.IndexOf(GCodeOrder, element);
        }

        private static string ExcludeComments(string innerString)
        {
            while (innerString.Any(c => c == '('))
            {
                var str1 = innerString.TakeWhile(c => c != '(');
                var str2 = innerString.SkipWhile(c => c != ')').Skip(1);
                innerString = new string(str1.Concat(str2).ToArray());
            }
            return innerString;
        }

        public string FileName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Свойства коллекций должны быть доступны только для чтения", Justification = "<Ожидание>")]
        public List<string> Strings { get; set; }
        public bool StringsPrepared { get; set; }
    }
}
