namespace OmniXaml.Wpf
{
    using System;
    using System.ComponentModel;

    public class TypeDescriptorContext : ITypeDescriptorContext
    {
        public object GetService(Type serviceType)
        {
            return null;
        }

        public bool OnComponentChanging()
        {
            throw new NotImplementedException();
        }

        public void OnComponentChanged()
        {
            throw new NotImplementedException();
        }

        public IContainer Container { get; }
        public object Instance { get; }
        public PropertyDescriptor PropertyDescriptor { get; }
    }
}