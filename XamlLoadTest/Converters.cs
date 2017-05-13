using System;
using OmniXaml;
using OmniXaml.Attributes;

namespace XamlLoadTest
{
    public static class Converters
    {
        [TypeConverterMember(typeof(ReferenceTarget))]
        public static Func<string, object> Convert = o =>
        {
            return new ReferenceTarget()
            {
                Value = 100,
            };
        };
    }
}