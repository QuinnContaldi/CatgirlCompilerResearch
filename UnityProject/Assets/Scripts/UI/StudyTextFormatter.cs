using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Meowra.UI
{
    // Presentation only: never diagnoses mistakes or supplies a correction.
    // A small C lexer is sufficient for the frozen snippets; this is not a compiler.
    public static class StudyTextFormatter
    {
        private const string Keyword = "#83BFFF";
        private const string Literal = "#E8BE91";
        private const string Number = "#B7DBA0";
        private const string Comment = "#A0B49A";
        private const string Location = "#A9CDD9";
        private const string Severity = "#FFADAD";
        private static readonly HashSet<string> Keywords = new HashSet<string>(
            ("auto break case char const continue default do double else enum extern float for goto " +
             "if inline int long register restrict return short signed sizeof static struct switch " +
             "typedef union unsigned void volatile while _Bool _Complex _Imaginary").Split(' '));

        private static readonly Regex Tokens = new Regex(
            @"(?<comment>//[^\r\n]*|/\*[\s\S]*?(?:\*/|$))|(?<literal>""(?:\\.|[^""\\])*""|'(?:\\.|[^'\\])*')|(?<directive>#[ \t]*[A-Za-z_]+)|(?<word>[A-Za-z_]\w*)|(?<number>\b(?:0[xX][\da-fA-F]+|\d+(?:\.\d+)?)(?:[uUlLfF]*)\b)");
        private static readonly Regex DiagnosticTokens = new Regex(
            @"(?<location>\b[\w./\\-]+\.c(?::\d+){0,2}:)|(?<severity>\b(?:error|warning|note):)|(?<literal>'[^'\r\n]*'|‘[^’\r\n]*’|`[^`\r\n]*`)");
        private static readonly Regex Excerpt = new Regex(@"^(\s*\d+\s*\|)(.*)$");

        public static string Code(string source) => Format(source ?? "", Tokens, false);

        public static string Feedback(string source)
        {
            var result = new StringBuilder();
            var lines = (source ?? "").Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (i > 0) result.Append('\n');
                var excerpt = Excerpt.Match(lines[i]);
                if (excerpt.Success)
                    result.Append(Paint(excerpt.Groups[1].Value, Location)).Append(Code(excerpt.Groups[2].Value));
                else
                    result.Append(Format(lines[i], DiagnosticTokens, true));
            }
            return result.ToString();
        }

        private static string Format(string source, Regex pattern, bool diagnostic)
        {
            var result = new StringBuilder();
            int position = 0;
            foreach (Match token in pattern.Matches(source))
            {
                result.Append(LiteralText(source.Substring(position, token.Index - position)));
                string color = null;
                if (token.Groups["literal"].Success) color = Literal;
                else if (diagnostic)
                    color = token.Groups["location"].Success ? Location : Severity;
                else if (token.Groups["comment"].Success) color = Comment;
                else if (token.Groups["number"].Success) color = Number;
                else if (token.Groups["directive"].Success || Keywords.Contains(token.Value)) color = Keyword;
                result.Append(color == null ? LiteralText(token.Value) : Paint(token.Value, color));
                position = token.Index + token.Length;
            }
            return result.Append(LiteralText(source.Substring(position))).ToString();
        }

        // Escape each opening bracket, including author-supplied </noparse> tags.
        // TMP then renders code such as <stdio.h> or "<color=red>" literally.
        private static string LiteralText(string text) => text.Replace("<", "<noparse><</noparse>");
        private static string Paint(string text, string color) => $"<color={color}>{LiteralText(text)}</color>";
    }
}
