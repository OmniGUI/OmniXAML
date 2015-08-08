namespace OmniXaml.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Xaml;

    public class TypeDescriptorContext : ITypeDescriptorContext
    {
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IXamlSchemaContextProvider))
            {
                return new XamlSchemaContentProvider(new XamlSchemaContext());
            }

            if (serviceType == typeof (IAmbientProvider))
            {
                return new AmbientProvider();
            }

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

    public class AmbientProvider : IAmbientProvider
    {
        public AmbientPropertyValue GetFirstAmbientValue(IEnumerable<XamlType> ceilingTypes, params XamlMember[] properties)
        {
            var context = new XamlSchemaContext();
            var type = context.GetXamlType(typeof (Setter));
            var member = type.GetMember("TargetType");
            return new AmbientPropertyValue(member, typeof (Button));
        }

        public object GetFirstAmbientValue(params XamlType[] types)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AmbientPropertyValue> GetAllAmbientValues(IEnumerable<XamlType> ceilingTypes, params XamlMember[] properties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAllAmbientValues(params XamlType[] types)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AmbientPropertyValue> GetAllAmbientValues(IEnumerable<XamlType> ceilingTypes, bool searchLiveStackOnly, IEnumerable<XamlType> types, params XamlMember[] properties)
        {
            throw new NotImplementedException();
        }
    }
}