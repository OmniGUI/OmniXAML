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

            if (serviceType == typeof (IXamlSchemaContextProvider))
            {
                return new XamlSchemaContentProvider(new XamlSchemaContextAdapter());
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
                var dp = type.GetDependencyProperty(name);
                return dp;
            }
        }      
    }

    public class XamlSchemaContextAdapter : XamlSchemaContext
    {
        public override XamlType GetXamlType(Type type)
        {
            return base.GetXamlType(type);
        }
    }
}