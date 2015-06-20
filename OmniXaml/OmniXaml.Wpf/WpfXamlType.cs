namespace OmniXaml.Wpf
{
    using System;
    using System.IO;
    using System.Text;
    using Typing;

    public class WpfXamlType : XamlType
    {
        private readonly IXamlTypeRepository typeRepository;

        public WpfXamlType(Type type, IXamlTypeRepository typeRepository) : base(type, typeRepository)
        {
            this.typeRepository = typeRepository;
        }

        protected override XamlMember LookupMember(string name)
        {
            return new WpfXamlMember(name, this, typeRepository, false);
        }

        protected override XamlMember LookupAttachableMember(string name)
        {
            return new WpfXamlMember(name, this, typeRepository, true);
        }
    }

    public static class XamlLoaderExtensions
    {
        public static object LoadFromFile(this IXamlLoader xamlLoader, string path)
        {
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return xamlLoader.Load(stream);
            }
        }
    }
}