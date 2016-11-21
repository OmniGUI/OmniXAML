namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Tests.Namespaces;

    public class InstanceCreator : IInstanceCreator
    {
        private readonly ISourceValueConverter converter;
        private readonly ObjectBuilderContext objectBuilderContext;
        private readonly ITypeDirectory directory;

        public InstanceCreator(ISourceValueConverter converter, ObjectBuilderContext context, ITypeDirectory directory)
        {
            this.converter = converter;
            this.objectBuilderContext = context;
            this.directory = directory;
        }

        public object Create(Type type, BuildContext context, IEnumerable<InjectableMember> injectableMembers = null)
        {
            if (type.GetTypeInfo().IsPrimitive)
            {
                return CreatePrimitive(type, injectableMembers.First().Value, context);
            }

            var ctor = SelectCtor(type, injectableMembers);
            return Call(ctor, context, injectableMembers ?? new List<InjectableMember>());            
        }

        private object CreatePrimitive(Type type, object o, BuildContext context)
        {
            var converterValueContext = new ConverterValueContext(type, o, objectBuilderContext, directory, context);
            return objectBuilderContext.SourceValueConverter.GetCompatibleValue(converterValueContext);
        }

        private object Call(ConstructorInfo ctor, BuildContext context, IEnumerable<InjectableMember> injectableMembers)
        {
            var requiredParams = ctor.GetParameters();
            var zip = requiredParams.Zip(injectableMembers, (p, m) => Convert(m.Value, p.ParameterType, context));
            return ctor.Invoke(zip.ToArray());
        }

        private object Convert(object value, Type targetType, BuildContext context)
        {
            return converter.GetCompatibleValue(new ConverterValueContext(targetType, value, objectBuilderContext, directory, context));
        }

        private static ConstructorInfo SelectCtor(Type type, IEnumerable<InjectableMember> injectableMembers)
        {
            var ctorsByNumOfArgs = from ctor in type.GetTypeInfo().DeclaredConstructors
                                          where ctor.IsPublic
                                          orderby ctor.GetParameters().Length descending
                                          select ctor;

            return ctorsByNumOfArgs.First(info => info.GetParameters().Length <= injectableMembers.Count());
        }
    }
}