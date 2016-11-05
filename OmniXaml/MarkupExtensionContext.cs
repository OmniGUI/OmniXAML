namespace OmniXaml
{
    public class ValueContext
    {

        public ObjectBuilderContext ObjectBuilderContext { get; }
        public Assignment Assignment { get; }
        public ITypeDirectory TypeDirectory { get; }
        public BuildContext BuildContext { get; set; }

        public ValueContext(Assignment assignment, ObjectBuilderContext objectBuilderContext, ITypeDirectory directory, BuildContext buildContext)
        {
            TypeDirectory = directory;
            BuildContext = buildContext;
            ObjectBuilderContext = objectBuilderContext;
            Assignment = assignment;
        }
    }
}