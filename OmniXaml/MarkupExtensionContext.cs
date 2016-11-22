namespace OmniXaml
{
    using TypeLocation;

    public class ExtensionValueContext
    {

        public ObjectBuilderContext ObjectBuilderContext { get; }
        public Assignment Assignment { get; }
        public ITypeDirectory TypeDirectory { get; }
        public BuildContext BuildContext { get; set; }

        public ExtensionValueContext(Assignment assignment, ObjectBuilderContext objectBuilderContext, ITypeDirectory directory, BuildContext buildContext)
        {
            TypeDirectory = directory;
            BuildContext = buildContext;
            ObjectBuilderContext = objectBuilderContext;
            Assignment = assignment;
        }
    }
}