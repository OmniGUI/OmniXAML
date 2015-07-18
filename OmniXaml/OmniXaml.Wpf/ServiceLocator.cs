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
                var dp = GetDependencyProperty(TargetObject.GetType(), markupExtensionContext.TargetProperty.Name);
                return dp;
            }
        }

        public static DependencyProperty GetDependencyProperty(Type type, string name)
        {
            FieldInfo fieldInfo = type.GetField(name + "Property", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            return (fieldInfo != null) ? (DependencyProperty)fieldInfo.GetValue(null) : null;
        }
    }
}