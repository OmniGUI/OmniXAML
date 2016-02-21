namespace OmniXaml
{
    using System;

    public class LoadException : Exception
    {
        public int LineNumber { get; private set; }
        public int LinePosition { get; private set; }

        public LoadException(string message, int lineNumber, int linePosition, Exception exception) : base(message, exception)
        {
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }
        public LoadException(int lineNumber, int linePosition, Exception exception) : base(string.Empty, exception)
        {
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }
    }
}