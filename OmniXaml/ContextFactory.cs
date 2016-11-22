namespace OmniXaml
{
    using System;
    using TypeLocation;

    public class ContextFactory : IContextFactory
    {
        private readonly ITypeDirectory directory;
        private readonly ObjectBuilderContext objectBuilderContext;

        public ContextFactory(ITypeDirectory directory, ObjectBuilderContext objectBuilderContext)
        {
            this.directory = directory;
            this.objectBuilderContext = objectBuilderContext;
        }

        public ConverterValueContext CreateConverterContext(Type type, object value, BuildContext buildContext)
        {
            return new ConverterValueContext(type, value, objectBuilderContext, directory, buildContext);
        }

        public ExtensionValueContext CreateExtensionContext(Assignment assignment, BuildContext buildContext)
        {
            return new ExtensionValueContext(assignment, objectBuilderContext, directory, buildContext );
        }
    }
}