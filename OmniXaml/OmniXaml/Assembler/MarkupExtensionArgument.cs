namespace OmniXaml.Assembler
{
    internal class MarkupExtensionArgument
    {
        public bool WasText { get; }
        public object Value { get; }

        public MarkupExtensionArgument(object value, bool wasText)
        {
            this.Value = value;
            this.WasText = wasText;
        }
    }
}