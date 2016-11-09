namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

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
            var ctor = SelectCtor(type);
            return Call(ctor, context, injectableMembers ?? new List<InjectableMember>());            
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

        private static ConstructorInfo SelectCtor(Type type)
        {
            return GetParameterlessCtor(type) ?? FirstCtorWithMostArguments(type);
        }

        private static ConstructorInfo FirstCtorWithMostArguments(Type type)
        {
            var ctorsByNumOfArgs = from ctor in type.GetTypeInfo().DeclaredConstructors
                where ctor.IsPublic
                orderby ctor.GetParameters().Length descending
                select ctor;

            return ctorsByNumOfArgs.First();
        }

        private static ConstructorInfo GetParameterlessCtor(Type type)
        {
            var parameterlessCtors = from ctor in type.GetTypeInfo().DeclaredConstructors
                where ctor.IsPublic && !ctor.GetParameters().Any()
                select ctor;

            return parameterlessCtors.FirstOrDefault();
        }
    }
}