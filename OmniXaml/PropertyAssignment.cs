namespace OmniXaml
{
    using System.Collections.Generic;

    public class PropertyAssignment
    {
        public Property Property { get; set; }
        public string SourceValue { get; set; }
        public IEnumerable<ConstructionNode> Children { get; set; }

        public override string ToString()
        {
            if (SourceValue != null)
            {
                return $@"{Property} = ""{SourceValue}""";
            }
            else
            {
                var formattedChildren = string.Join(", ", Children);
                return $"{Property} = {formattedChildren}";
            }
        }
    }
}