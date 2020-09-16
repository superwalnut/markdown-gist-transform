using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace MarkdownToGist.Extensions
{
    public static class RegexExtensions
    {
        public static string Replace(this MatchCollection matches, string source, string replacement)
        {
            foreach (var match in matches.Cast<Match>())
            {
                source = match.Replace(source, replacement);
            }
            return source;
        }
        public static string Replace(this Match match, string source, string replacement)
        {
            return source.Substring(0, match.Index) + replacement + source.Substring(match.Index + match.Length);
        }
    }
}
