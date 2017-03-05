namespace OmniXaml
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using TypeLocation;

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
            if (IsCreatableUsingConversion(type))
            {
                return CreateUsingConversion(type, injectableMembers.First().Value, context);
            }

            var ctor = SelectConstructor(type, injectableMembers);
            return ctor == null
                ? null
                : InvokeSelectedConstructor(ctor, context, injectableMembers ?? new List<InjectableMember>());
        }

        private static bool IsCreatableUsingConversion(Type type)
        {
            return type.GetTypeInfo().IsPrimitive || type == typeof(string) || type == typeof(decimal);
        }

        private object CreateUsingConversion(Type type, object o, BuildContext context)
        {
            var converterValueContext = new ConverterValueContext(type, o, objectBuilderContext, directory, context);
            return objectBuilderContext.SourceValueConverter.GetCompatibleValue(converterValueContext);
        }

        private object InvokeSelectedConstructor(ConstructorInfo ctor, BuildContext context, IEnumerable<InjectableMember> injectableMembers)
        {
            var requiredParams = ctor.GetParameters();
            var zip = requiredParams.Zip(injectableMembers, (p, m) => Convert(m.Value, p.ParameterType, context));
            return ctor.Invoke(zip.ToArray());
        }

        private object Convert(object value, Type targetType, BuildContext context)
        {
            return converter.GetCompatibleValue(new ConverterValueContext(targetType, value, objectBuilderContext, directory, context));
        }

        private static ConstructorInfo SelectConstructor(Type type, IEnumerable<InjectableMember> injectableMembers)
        {
            var ctorsByNumOfArgs = from ctor in type.GetTypeInfo().DeclaredConstructors
                where ctor.IsPublic
                orderby ctor.GetParameters().Length descending
                select ctor;

            return ctorsByNumOfArgs.FirstOrDefault(info => info.GetParameters().Length <= injectableMembers.Count());
        }
    }
}