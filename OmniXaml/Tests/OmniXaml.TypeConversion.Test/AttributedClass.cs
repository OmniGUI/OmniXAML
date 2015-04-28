using System.ComponentModel;

namespace Perspex.Markup.Test
{
    [TypeConverter(typeof(TypeConverterDummy))]
    public class AttributedClass
    {
    }
}