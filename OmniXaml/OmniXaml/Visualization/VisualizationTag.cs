namespace OmniXaml.Visualization
{
    using System.Diagnostics;

    [DebuggerDisplay("{Name}")]
    public class VisualizationTag
    {
        public string Name { get; private set; }
        public int Level { get; set; }

        public VisualizationTag(string name, int level)
        {
            Name = name;
            Level = level;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}