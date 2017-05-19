namespace OmniXaml.Rework
{
    public class MutablePipelineUnit
    {
        public bool Handled { get; set; }
        public ConstructionNode ParentNode { get; }
        public object Value { get; set; }

        public MutablePipelineUnit(ConstructionNode parentNode, object value)
        {
            ParentNode = parentNode;
            Value = value;
        }
    }
}