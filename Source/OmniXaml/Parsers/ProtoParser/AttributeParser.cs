namespace OmniXaml.Parsers.ProtoParser
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Typing;

    internal class AttributeParser
    {
        private readonly IXmlReader reader;

        private string specialPrefix;

        public AttributeParser(IXmlReader reader)
        {
            this.reader = reader;
        }

        public AttributeFeed Read()
        {
            var attributeAssignments = new Collection<AttributeAssignment>();

            if (reader.MoveToFirstAttribute())
            {
                do
                {
                    attributeAssignments.Add(GetAttribute());

                } while (reader.MoveToNextAttribute());

                reader.MoveToElement();
            }

            ScanSpecialNsAndSetIfNoPreviousValue(attributeAssignments);

            return new AttributeFeed(attributeAssignments, specialPrefix);
        }

        private void ScanSpecialNsAndSetIfNoPreviousValue(Collection<AttributeAssignment> attributeAssignments)
        {
            if (specialPrefix != null)
            {
                return;
            }

            var attributeAssignment = attributeAssignments.FirstOrDefault(IsNsThatMatchesSpecialValue);
            if (attributeAssignment!=null)
            {
                specialPrefix = attributeAssignment.Locator.PropertyName;
            }
        }

        private static bool IsNsThatMatchesSpecialValue(AttributeAssignment assignment)
        {
            return assignment.Locator.IsNsPrefixDefinition && assignment.Value == CoreTypes.SpecialNamespace;
        }

        private AttributeAssignment GetAttribute()
        {
            return new AttributeAssignment(PropertyLocator.Parse(reader.Name), reader.Value);
        }
    }
}