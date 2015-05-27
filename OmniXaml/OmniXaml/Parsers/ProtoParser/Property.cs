namespace OmniXaml.Parsers.ProtoParser
{
    using System;
    using Glass;

    internal class Property
    {
        public Property(string longName)
        {
            Guard.ThrowIfNull(longName, nameof(longName));

            var colonPosition = longName.IndexOf(":", StringComparison.Ordinal);
            if (colonPosition != -1)
            {
                Prefix = longName.Substring(0, colonPosition);
                Name = longName.Substring(colonPosition + 1, longName.Length - colonPosition - 1);
            }
            else
            {
                Prefix = longName;
                Name = string.Empty;
            }
        }

        public string Name { get; }
        public string Prefix { get; private set; }
    }
}