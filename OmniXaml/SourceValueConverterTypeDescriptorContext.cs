namespace OmniXaml
{
    using System;
    using System.ComponentModel;

    public class SourceValueConverterTypeDescriptorContext : ITypeDescriptorContext
    {
        private readonly ConverterValueContext valueContext;

        public SourceValueConverterTypeDescriptorContext(ConverterValueContext valueContext)
        {
            this.valueContext = valueContext;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(ConverterValueContext))
            {
                return valueContext;
            }

            throw new InvalidOperationException("The only service provided by this context is ConverterValueContext. It provides the XAML parsing context.");
        }

        public void OnComponentChanged()
        {
        }

        public bool OnComponentChanging()
        {
            return false;
        }

        public IContainer Container { get; }
        public object Instance { get; }
        public PropertyDescriptor PropertyDescriptor { get; }
    }
}