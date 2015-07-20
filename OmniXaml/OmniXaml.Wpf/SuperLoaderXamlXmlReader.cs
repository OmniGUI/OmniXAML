namespace OmniXaml.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Markup;
    using System.Xaml;
    using SystemXamlNsDeclaration = System.Xaml.NamespaceDeclaration;
    using SystemXamlType = System.Xaml.XamlType;
    using SystemXamlNodeType = System.Xaml.XamlNodeType;
    using XamlReader = System.Xaml.XamlReader;

    public class SuperLoaderXamlXmlReader : XamlReader, IXamlIndexingReader
    {
        private readonly IEnumerator<XamlNode> nodeStream;
        private readonly TemplateContent templateContent;
        private bool hasReadSuccess;

        public SuperLoaderXamlXmlReader(TemplateContent templateContent)
        {
            this.templateContent = templateContent;
            SchemaContext = new XamlSchemaContext();
            nodeStream = templateContent.Nodes.GetEnumerator();
        }

        public override SystemXamlNodeType NodeType => XamlTypeConversion.ToWpf(nodeStream.Current.NodeType);
        public override bool IsEof => !hasReadSuccess;
        public override SystemXamlNsDeclaration Namespace => XamlTypeConversion.ToWpf(nodeStream.Current.NamespaceDeclaration);
        public override SystemXamlType Type => XamlTypeConversion.ToWpf(nodeStream.Current.XamlType, SchemaContext);
        public override object Value => nodeStream.Current.Value;
        public override XamlMember Member => XamlTypeConversion.ToWpf(nodeStream.Current.Member, SchemaContext);
        public override XamlSchemaContext SchemaContext { get; }
        public int Count => templateContent.Nodes.Count();
        public int CurrentIndex { get; set; }

        public override bool Read()
        {
            hasReadSuccess = nodeStream.MoveNext();
            if (hasReadSuccess)
            {
                CurrentIndex++;
            }

            return hasReadSuccess;
        }
    }

    internal class NastyXamlMember : XamlMember, IProvideValueTarget
    {
        private readonly XamlMember member;
        private readonly object targetObject;
        private readonly object targetProperty;

        public NastyXamlMember(XamlMember member, XamlSchemaContext context) : base(member.DeclaringType.UnderlyingType.GetProperty(member.Name), context)
        {
            this.member = member;
        }

        public object TargetObject
        {
            get { return targetObject; }
        }

        public object TargetProperty => GetDependencyProperty(member.DeclaringType.UnderlyingType, member.Name);

        public static DependencyProperty GetDependencyProperty(Type type, string name)
        {
            var dpPropName = name + "Property";
            FieldInfo fieldInfo = type.GetField(dpPropName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            return (DependencyProperty)fieldInfo?.GetValue(null);
        }
    }

    internal class MarkupExtensionXamlType : SystemXamlType, IProvideValueTarget
    {
        private readonly object targetObject;
        private readonly object targetProperty;

        public MarkupExtensionXamlType(Type type, XamlSchemaContext schemaContext) : base(type, schemaContext)
        {            
        }

        public object TargetObject
        {
            get { return targetObject; }
        }

        public object TargetProperty
        {
            get { return targetProperty; }
        }
    }
}