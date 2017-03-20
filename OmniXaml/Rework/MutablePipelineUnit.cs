namespace OmniXaml.Rework
{
    public class MutablePipelineUnit
    {
        public bool Handled { get; set; }
        public object Value { get; set; }

        public MutablePipelineUnit(object value)
        {
            Value = value;
        }
    }
}