namespace OmniXaml.Parsers.ProtoParser
{
    using System.Linq;
    using System.Text;

    public class TextBuffer
    {
        private StringBuilder stringBuilder;

        private const char Tab = '\t';
        private const char Space = ' ';
        private const char NewLine = '\n';
        private const char Backspace = '\b';

        public TextBuffer()
        {
            stringBuilder = new StringBuilder();
            IsWhiteSpaceOnly = true;
        }

        public bool IsEmpty => stringBuilder.Length == 0;

        public string Text => stringBuilder.ToString();

        public bool IsWhiteSpaceOnly { get; private set; }

        public void Append(string text, bool trimLeadingWhitespace, bool replaceCrLf = true)
        {
            var isWhitespace = IsWhitespace(text);

            if (isWhitespace)
            {
                if (IsEmpty && !trimLeadingWhitespace)
                {
                    stringBuilder.Append(Space);
                }
            }
            else
            {
                var isWhitespaceChar = IsWhitespaceChar(text[0]);
                var whitespaceChar = IsWhitespaceChar(text[text.Length - 1]);
                var lastIsAlsoWhitespace = false;
                var str = CollapseWhitespace(text);

                if (stringBuilder.Length > 0)
                {
                    if (IsWhiteSpaceOnly)
                    {
                        stringBuilder = new StringBuilder();
                    }
                    else if (IsWhitespaceChar(stringBuilder[stringBuilder.Length - 1]))
                    {
                        lastIsAlsoWhitespace = true;
                    }
                }

                if (isWhitespaceChar && !trimLeadingWhitespace && !lastIsAlsoWhitespace)
                {
                    stringBuilder.Append(Space);
                }

                stringBuilder.Append(str);

                if (whitespaceChar)
                {
                    stringBuilder.Append(Space);
                }
            }

            IsWhiteSpaceOnly = IsWhiteSpaceOnly && isWhitespace;
        }

        private static bool IsWhitespace(string text)
        {
            return text.All(IsWhitespaceChar);
        }

        private static bool IsWhitespaceChar(char ch)
        {
            if (ch != Space && ch != Tab && ch != Backspace)
            {
                return ch == NewLine;
            }
            return true;
        }

        private static string CollapseWhitespace(string text)
        {
            var stringBuilder = new StringBuilder(text.Length);
            var start = 0;
            while (start < text.Length)
            {
                var ch = text[start];
                if (!IsWhitespaceChar(ch))
                {
                    stringBuilder.Append(ch);
                    start++;
                }
                else
                {
                    var end = start;

                    do
                    {
                    } while (end++ < text.Length && IsWhitespaceChar(text[end]));

                    if (start != 0 && end != text.Length)
                    {
                        stringBuilder.Append(Space);
                    }
                    start = end;
                }
            }
            return stringBuilder.ToString();
        }
    }
}