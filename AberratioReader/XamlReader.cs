using System.IO;
using System.Xml;

namespace AberratioReader
{
    public class XamlReader
    {
        private readonly ITypeFactory typeFactory;
        private readonly ITypeProvider typeProvider;

        public XamlReader(ITypeFactory typeFactory, ITypeProvider typeProvider)
        {
            this.typeFactory = typeFactory;
            this.typeProvider = typeProvider;
        }

        public object Load(MemoryStream stream)
        {
            var xmlReader = XmlReader.Create(stream);
            xmlReader.Read();
            xmlReader.ReadStartElement();

            var type = typeProvider.GetType(xmlReader.Name, string.Empty, string.Empty);

            return typeFactory.Create(type);            
        }
    }
}
