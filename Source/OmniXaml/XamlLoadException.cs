namespace OmniXaml
{
    using System;

    public class XamlLoadException : Exception
    {
        public int CurrentLine { get; private set; }
        public int CurrentChar { get; private set; }

        public XamlLoadException(string message, int currentLine, int currentChar, Exception exception) : base(message, exception)
        {
            CurrentLine = currentLine;
            CurrentChar = currentChar;
        }
        public XamlLoadException(int currentLine, int currentChar, Exception exception) : base(string.Empty, exception)
        {
            CurrentLine = currentLine;
            CurrentChar = currentChar;
        }
    }
}