using System.Text.RegularExpressions;

namespace RegexPrecompiler
{
    class Program
    {
        //static PrecompiledRegex.CNCRegex cNCRegex = new PrecompiledRegex.CNCRegex();
        static void Main()
        {
            Regex.CompileToAssembly(new RegexCompilationInfo[]
            {
                new RegexCompilationInfo(pattern: @"([GgMm][0-9]+[.,][0-9]+|[GgMm][0-9]+)|([AaBbCcUuVvWwXxYyZzRrIiJjKk][-+]?[0-9]+[.,]?\d*|[SsFfDdHh][0-9]+)|([Tt][0-9]+)",
                    RegexOptions.CultureInvariant, name: "CNCRegex", fullnamespace: "PrecompiledRegex", ispublic: true),
                new RegexCompilationInfo(pattern: @"([Gg][0-9]+[.,][0-9]+|[Gg][0-9]+)", RegexOptions.CultureInvariant, name: "CNCGCodes", fullnamespace: "PrecompiledRegex", ispublic: true),
                new RegexCompilationInfo(pattern: @"([Mm][0-9]+[.,][0-9]+|[Mm][0-9]+)", RegexOptions.CultureInvariant, name: "CNCMCodes", fullnamespace: "PrecompiledRegex", ispublic: true),
                new RegexCompilationInfo(pattern: @"([Tt][0-9]+[.,][0-9]+|[Tt][0-9]+)", RegexOptions.CultureInvariant, name: "CNCTCode", fullnamespace: "PrecompiledRegex", ispublic: true),
                new RegexCompilationInfo(pattern: @"([Ss][0-9]+[.,][0-9]+|[Ss][0-9]+)", RegexOptions.CultureInvariant, name: "CNCSCode", fullnamespace: "PrecompiledRegex", ispublic: true),
                new RegexCompilationInfo(pattern: @"([Ff][0-9]+[.,][0-9]+|[Ff][0-9]+)", RegexOptions.CultureInvariant, name: "CNCFCode", fullnamespace: "PrecompiledRegex", ispublic: true),
                new RegexCompilationInfo(pattern: @"([DdHh][-+]?[0-9]+[.,]?\d*)", RegexOptions.CultureInvariant, name: "CNCDHCode", fullnamespace: "PrecompiledRegex", ispublic: true),
                new RegexCompilationInfo(pattern: @"([AaBbCcUuVvWwXxYyZz][-+]?[0-9]+[.,]?\d*)", RegexOptions.CultureInvariant, name: "CNCCoordinates", fullnamespace: "PrecompiledRegex", ispublic: true),
                new RegexCompilationInfo(pattern: @"([IJKRijkr][-+]?[0-9]+[.,]?\d*)", RegexOptions.CultureInvariant, name: "CNCArcValues", fullnamespace: "PrecompiledRegex", ispublic: true),
            }, new System.Reflection.AssemblyName("CNCPrecompiledRegex"));

        }
    }
}
