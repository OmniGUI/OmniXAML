namespace OmniXaml
{
    using System;

    public class XamlLoadException : Exception
    {
        public int LineNumber { get; private set; }
        public int LinePosition { get; private set; }

        public XamlLoadException(string message, int lineNumber, int linePosition, Exception exception) : base(message, exception)
        {
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }
        public XamlLoadException(int lineNumber, int linePosition, Exception exception) : base(string.Empty, exception)
        {
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }
    }
}