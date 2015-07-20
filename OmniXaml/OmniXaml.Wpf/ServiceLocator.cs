namespace OmniXaml.Wpf
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Markup;
    using System.Xaml;

    public class ServiceLocator : IServiceProvider, IProvideValueTarget
    {
        private readonly MarkupExtensionContext markupExtensionContext;

        public ServiceLocator(MarkupExtensionContext markupExtensionContext)
        {
            this.markupExtensionContext = markupExtensionContext;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof (IProvideValueTarget))
            {
                return this;
            }

            if (serviceType == typeof(IXamlObjectWriterFactory))
            {
                return new ObjectWriterFactory();
            }

            if (serviceType == typeof(IRootObjectProvider))
            {
                return new RootObjectProvider();
            }

            throw new InvalidOperationException($"Cannot locate the type {serviceType}");
        }

        public object TargetObject => markupExtensionContext.TargetObject;

        public object TargetProperty
        {
            get
            {
                var name = markupExtensionContext.TargetProperty.Name;
                var type = TargetObject.GetType();
                var dp = GetDependencyProperty(type, name);
                return dp;
            }
        }

        public static DependencyProperty GetDependencyProperty(Type type, string name)
        {
            var dpPropName = name + "Property";
            FieldInfo fieldInfo = type.GetField(dpPropName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            return (DependencyProperty) fieldInfo?.GetValue(null);
        }
    }
}