using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MarkdownToGist.Extensions
{
    public static class MarkdownExtensions
    {
        private static Regex _metaRegex = new Regex(@"(---[a-z\W]*\n[\s\S]*?\n---)");
        public static (string, string[], string) ToTitleTagsBody(this string content)
        {
            if (_metaRegex.IsMatch(content))
            {
                var meta = _metaRegex.Match(content);

                string line = null, title = "", tagVal = "", body = "";
                var titleRegex = new Regex(@"\s*title\s*:");
                var tagsRegex = new Regex(@"\s*tags\s*:");

                body = content.Replace(meta.Value, "");

                StringReader strReader = new StringReader(meta.Value);
                while (true)
                {
                    line = strReader.ReadLine();
                    if (line == null)
                        break;

                    if (titleRegex.IsMatch(line))
                    {
                        title = titleRegex.Match(line).Replace(line, "").Replace("\"", "");
                    }

                    if (tagsRegex.IsMatch(line))
                    {
                        tagVal = tagsRegex.Match(line).Replace(line, "");
                    }
                }

                var tags = tagVal.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                return (title, tags, body);
            }

            return (null, null, null);
        }
    }
}
